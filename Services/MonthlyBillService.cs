using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class MonthlyBillService : IMonthlyBillService
    {
        private readonly IMonthlyBillRepository _repo;
        private readonly LaudaryMis.Helpers.PortalCalendar _calendar;

        public MonthlyBillService(IMonthlyBillRepository repo, LaudaryMis.Helpers.PortalCalendar calendar)
        {
            _repo = repo;
            _calendar = calendar;
        }

        // ──────────────────────────────────────────────────────
        // Form Load — agreement info + auto WPR score
        // ──────────────────────────────────────────────────────
        public async Task<MonthlyBillVM> LoadBillFormAsync(
            int hospitalId, int month, int year)
        {
            if (_calendar.IsBeforeStart(month, year))
                throw new Exception(_calendar.BeforeStartMessage("Bill"));

            // 1. Existing bill check
            var existing = await _repo.GetBillByHospitalMonthAsync(
                hospitalId, month, year);
            if (existing != null)
                return await GetBillDetailAsync(existing.Id)
                       ?? throw new Exception("Bill load nahi hua.");

            // 2. Agreement info
            var agr = await _repo.GetAgreementInfoAsync(hospitalId)
                      ?? throw new Exception(
                             "Koi active agreement nahi mila is hospital ke liye.");

            // 3. WPR auto-calculate
            var (avgScore, weeksCount) = await _repo.GetWPRAvgScoreAsync(
                hospitalId, month, year);

            var vm = new MonthlyBillVM
            {
                AgreementId = agr.AgreementId,
                HospitalId = hospitalId,
                HospitalName = agr.HospitalName,
                ContractNo = agr.ContractNo,
                BillingMonth = month,
                BillingYear = year,
                SanctionedBeds = agr.SanctionedBeds,
                RatePerBedPerYear = agr.RatePerBedPerYear,
                GSTPercent = 18m,
                AutoCalculatedScore = avgScore,
                WPRWeeksFound = weeksCount,
                WPRAvgScore = avgScore ?? 0,
                IsScoreOverridden = false,
                Status = "Draft"
            };

            ComputeAmounts(vm);
            return vm;
        }

        // ──────────────────────────────────────────────────────
        // Core Calculation — ek jagah, sab jagah use hoga
        // ──────────────────────────────────────────────────────
        // Contract (Part-III, Payment Mechanism):
        //   Monthly gross bill = beds × rate per bed per year ÷ 12 (GST ke bina)
        //   WPR band % gross bill pe lagta hai → Base Payable
        //   GST (18%) base payable pe alag se judta hai
        //   TDS (2%, Sec 194C) base payable (GST ke bina) pe katta hai
        //   Net = Base + GST − TDS − extra deductions
        public void ComputeAmounts(MonthlyBillVM vm)
        {
            vm.AnnualValueExGST = vm.SanctionedBeds * vm.RatePerBedPerYear;
            vm.AnnualValueInGST = Round2(vm.AnnualValueExGST
                                    * (1 + vm.GSTPercent / 100));
            vm.MonthlyGrossAmount = Round2(vm.AnnualValueExGST / 12);

            vm.PaymentBandPercent = GetPaymentBandPercent(vm.WPRAvgScore);

            vm.BasePayableAmount = Round2(vm.MonthlyGrossAmount
                                   * vm.PaymentBandPercent / 100);
            vm.GSTAmount = Round2(vm.BasePayableAmount * vm.GSTPercent / 100);
            vm.TDSAmount = Round2(vm.BasePayableAmount * 0.02m);
            vm.NetPayableAmount = vm.BasePayableAmount
                                   + vm.GSTAmount
                                   - vm.TDSAmount
                                   - vm.AdditionalDeductions;
        }

        // WPR monthly average → % of gross bill (contract bands:
        // 0-20 nil, 21-40 40%, 41-60 60%, 61-70 80%, 71-80 90%, 81-100 100%).
        // sp_GetPaymentCalculation mein bhi yahi boundaries hain.
        public static decimal GetPaymentBandPercent(decimal avgScore) => avgScore switch
        {
            >= 81 => 100m,
            >= 71 => 90m,
            >= 61 => 80m,
            >= 41 => 60m,
            >= 21 => 40m,
            _ => 0m
        };

        private static decimal Round2(decimal value) =>
            Math.Round(value, 2, MidpointRounding.AwayFromZero);

        // ──────────────────────────────────────────────────────
        // Save Draft
        // ──────────────────────────────────────────────────────
        public async Task<(bool Success, string Message, int BillId)>
            SaveDraftAsync(MonthlyBillVM vm, int userId)
        {
            if (_calendar.IsBeforeStart(vm.BillingMonth, vm.BillingYear))
                return (false, _calendar.BeforeStartMessage("Bill"), 0);

            // Override reason mandatory check
            if (vm.IsScoreOverridden &&
                string.IsNullOrWhiteSpace(vm.OverrideReason))
                return (false,
                    "Score override karne ka reason daalna zaroori hai.", 0);

            // Duplicate check
            var existing = await _repo.GetBillByHospitalMonthAsync(
                vm.HospitalId, vm.BillingMonth, vm.BillingYear);

            if (existing != null &&
                existing.Status is not ("Draft" or "HospitalRejected" or "CMSRejected"))
                return (false,
                    $"Is month ka bill already '{existing.Status}' " +
                    $"status mein hai.", 0);

            // Agreement, beds and rate come from the database, never from the form.
            var agr = await _repo.GetAgreementInfoByProviderHospitalAsync(
                vm.ProviderId, vm.HospitalId);
            if (agr == null)
                return (false,
                    "Is Provider-Hospital ke beech koi active agreement nahi hai.", 0);
            vm.AgreementId = agr.AgreementId;
            vm.SanctionedBeds = agr.SanctionedBeds;
            vm.RatePerBedPerYear = agr.RatePerBedPerYear;
            vm.GSTPercent = 18m;

            if (existing != null && existing.ProviderId != vm.ProviderId)
                return (false, "Yeh bill aapka nahi hai.", 0);

            ComputeAmounts(vm);

            var bill = new MonthlyBill
            {
                AgreementId = vm.AgreementId,
                HospitalId = vm.HospitalId,
                ProviderId=vm.ProviderId,
                BillingMonth = (byte)vm.BillingMonth,
                BillingYear = (short)vm.BillingYear,
                SanctionedBeds = vm.SanctionedBeds,
                RatePerBedPerYear = vm.RatePerBedPerYear,
                GSTPercent = vm.GSTPercent,
                WPRAvgScore = vm.WPRAvgScore,
                WPRWeeksConsidered = (byte)vm.WPRWeeksFound,
                IsScoreOverridden = vm.IsScoreOverridden,
                OverrideReason = vm.OverrideReason,
                AnnualValueExGST = vm.AnnualValueExGST,
                AnnualValueInGST = vm.AnnualValueInGST,
                MonthlyGrossAmount = vm.MonthlyGrossAmount,
                PaymentBandPercent = vm.PaymentBandPercent,
                BasePayableAmount = vm.BasePayableAmount,
                TDSPercent = 2m,
                TDSAmount = vm.TDSAmount,
                AdditionalDeductions = vm.AdditionalDeductions,
                DeductionRemarks = vm.DeductionRemarks,
                NetPayableAmount = vm.NetPayableAmount,
                Status = "Draft",
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };

            int billId;
            if (existing == null)
            {
                billId = await _repo.InsertBillAsync(bill);
                await _repo.InsertWorkflowLogAsync(new BillWorkflowLog
                {
                    BillId = billId,
                    FromStatus = null,
                    ToStatus = "Draft",
                    ActionBy = userId,
                    ActionAt = DateTime.Now,
                    Remarks = "Bill create kiya"
                });
            }
            else
            {
                bill.Id = existing.Id;
                await _repo.UpdateBillAsync(bill);
                billId = existing.Id;
            }

            return (true, "Draft save ho gaya.", billId);
        }

        // ──────────────────────────────────────────────────────
        // CMS Approve / Reject (sirf Hospital-approved bills)
        // ──────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)>
            CMSActionAsync(int billId, int cmsUserId,
                           bool approve, string? remarks)
        {
            var bill = await _repo.GetBillByIdAsync(billId);
            if (bill == null)
                return (false, "Bill nahi mila.");
            if (bill.Status != "HospitalApproved")
                return (false,
                    "Sirf 'HospitalApproved' status ke bills pe CMS action ho sakta hai.");
            if (!approve && string.IsNullOrWhiteSpace(remarks))
                return (false,
                    "Reject karte waqt reason likhna zaroori hai.");

            string newStatus = approve ? "CMSApproved" : "CMSRejected";
            string logRemark = approve
                ? $"CMS ne approve kiya. Net Payable: " +
                  $"₹{bill.NetPayableAmount:N2}"
                : $"CMS ne reject kiya. Reason: {remarks}";

            await _repo.UpdateBillStatusAsync(
                billId, newStatus, cmsUserId, remarks,
                cmsActionAt: DateTime.Now, cmsActionBy: cmsUserId);

            await _repo.InsertWorkflowLogAsync(new BillWorkflowLog
            {
                BillId = billId,
                FromStatus = "HospitalApproved",
                ToStatus = newStatus,
                ActionBy = cmsUserId,
                ActionAt = DateTime.Now,
                Remarks = logRemark
            });

            return (true, approve
                ? $"✅ Bill approve ho gaya. Net Payable: " +
                  $"₹{bill.NetPayableAmount:N2}"
                : "❌ Bill reject kar diya gaya.");
        }

        // ──────────────────────────────────────────────────────
        // Get Bill Detail (with workflow log)
        // ──────────────────────────────────────────────────────
        public async Task<MonthlyBillVM?> GetBillDetailAsync(int billId)
        {
            var bill = await _repo.GetBillByIdAsync(billId);
            if (bill == null) return null;

            var agr = await _repo.GetAgreementInfoAsync(bill.HospitalId);
            var log = await _repo.GetWorkflowLogAsync(billId);

            return new MonthlyBillVM
            {
                BillId = bill.Id,
                AgreementId = bill.AgreementId,
                HospitalId = bill.HospitalId,
                ProviderId = bill.ProviderId,
                HospitalName = agr?.HospitalName ?? string.Empty,
                ContractNo = agr?.ContractNo ?? string.Empty,
                BillingMonth = bill.BillingMonth,
                BillingYear = bill.BillingYear,
                SanctionedBeds = bill.SanctionedBeds,
                RatePerBedPerYear = bill.RatePerBedPerYear,
                GSTPercent = bill.GSTPercent,
                WPRAvgScore = bill.WPRAvgScore,
                WPRWeeksFound = bill.WPRWeeksConsidered,
                IsScoreOverridden = bill.IsScoreOverridden,
                OverrideReason = bill.OverrideReason,
                AnnualValueExGST = bill.AnnualValueExGST,
                AnnualValueInGST = bill.AnnualValueInGST,
                MonthlyGrossAmount = bill.MonthlyGrossAmount,
                PaymentBandPercent = bill.PaymentBandPercent,
                BasePayableAmount = bill.BasePayableAmount,
                GSTAmount = Round2(bill.BasePayableAmount * bill.GSTPercent / 100),
                TDSAmount = bill.TDSAmount,
                AdditionalDeductions = bill.AdditionalDeductions,
                DeductionRemarks = bill.DeductionRemarks,
                NetPayableAmount = bill.NetPayableAmount,
                Status = bill.Status,
                WorkflowLog = log.ToList()
            };
        }

        public async Task<IEnumerable<BillListItemVM>> GetAllBillsAsync(
            string? status = null, int? hospitalId = null)
            => await _repo.GetAllBillsAsync(status, hospitalId);

        public async Task<IEnumerable<BillListItemVM>> GetHospitalBillsAsync(
            int hospitalId)
            => await _repo.GetHospitalBillsAsync(hospitalId);

        public async Task<MonthlyBillVM> LoadProviderBillFormAsync(
  int providerId, int hospitalId, int month, int year)
        {
            if (_calendar.IsBeforeStart(month, year))
                throw new Exception(_calendar.BeforeStartMessage("Bill"));

            // 1. Existing bill check karo
            var existing = await _repo.GetBillByProviderHospitalMonthAsync(
                providerId, hospitalId, month, year);
            if (existing != null)
                return await GetBillDetailAsync(existing.Id)
                       ?? throw new Exception("Bill load nahi hua.");

            // 2. Agreement info (Provider + Hospital dono se)
            var agr = await _repo.GetAgreementInfoByProviderHospitalAsync(
                          providerId, hospitalId)
                      ?? throw new Exception(
                             "Koi active agreement nahi mila is " +
                             "Provider-Hospital ke beech mein.");

            // 3. WPR auto-calculate
            var (avgScore, weeksCount) = await _repo.GetWPRAvgScoreAsync(
                hospitalId, month, year);

            var vm = new MonthlyBillVM
            {
                AgreementId = agr.AgreementId,
                HospitalId = hospitalId,
                ProviderId = providerId,
                HospitalName = agr.HospitalName,
                ProviderName = agr.ProviderName,
                ContractNo = agr.ContractNo,
                BillingMonth = month,
                BillingYear = year,
                SanctionedBeds = agr.SanctionedBeds,
                RatePerBedPerYear = agr.RatePerBedPerYear,
                GSTPercent = 18m,
                AutoCalculatedScore = avgScore,
                WPRWeeksFound = weeksCount,
                WPRAvgScore = avgScore ?? 0,
                IsScoreOverridden = false,
                Status = "Draft"
            };

            ComputeAmounts(vm);
            return vm;
        }

        // ──────────────────────────────────────────────────────
        // Provider → Hospital ko submit karo
        // ──────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)>
            SubmitToHospitalAsync(int billId, int providerId, int userId)
        {
            var bill = await _repo.GetBillByIdAsync(billId);
            if (bill == null)
                return (false, "Bill nahi mila.");
            if (bill.ProviderId != providerId)
                return (false, "Yeh bill aapka nahi hai.");
            if (bill.Status is not ("Draft" or "HospitalRejected" or "CMSRejected"))
                return (false,
                    $"Bill '{bill.Status}' status mein hai — submit nahi ho sakta.");
            if (bill.WPRAvgScore <= 0)
                return (false,
                    "WPR Score 0 hai. Submit karne se pehle score check karein.");

            await _repo.UpdateBillStatusAsync(
                billId, "HospitalSubmitted", userId, null);

            await _repo.InsertWorkflowLogAsync(new BillWorkflowLog
            {
                BillId = billId,
                FromStatus = bill.Status,
                ToStatus = "HospitalSubmitted",
                ActionBy = userId,
                ActionAt = DateTime.Now,
                Remarks = "Provider ne Hospital ko submit kiya"
            });

            return (true, "Bill Hospital ko bhej diya gaya. ✅");
        }

        // ──────────────────────────────────────────────────────
        // Hospital → Approve / Reject Provider ka bill
        // ──────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)>
            HospitalActionAsync(int billId, int hospitalUserId,
                                bool approve, string? remarks, int hospitalId)
        {
            var bill = await _repo.GetBillByIdAsync(billId);
            if (bill == null)
                return (false, "Bill nahi mila.");
            if (bill.HospitalId != hospitalId)
                return (false, "Yeh bill aapke hospital ka nahi hai.");
            if (bill.Status != "HospitalSubmitted")
                return (false,
                    "Sirf 'HospitalSubmitted' status ke bills verify ho sakte hain.");
            if (!approve && string.IsNullOrWhiteSpace(remarks))
                return (false,
                    "Reject karte waqt reason likhna zaroori hai.");

            // Hospital approve → CMS ke paas bhejo
            string newStatus = approve ? "HospitalApproved" : "HospitalRejected";

            await _repo.UpdateHospitalActionAsync(
                billId, approve, hospitalUserId, remarks);

            await _repo.InsertWorkflowLogAsync(new BillWorkflowLog
            {
                BillId = billId,
                FromStatus = "HospitalSubmitted",
                ToStatus = newStatus,
                ActionBy = hospitalUserId,
                ActionAt = DateTime.Now,
                Remarks = approve
                    ? $"Hospital ne verify kiya. Net: ₹{bill.NetPayableAmount:N2}"
                    : $"Hospital ne reject kiya. Reason: {remarks}"
            });

            return (true, approve
                ? "✅ Bill verify ho gaya — Ab CMS approve karega."
                : "❌ Bill reject kar diya — Provider ko wapas bhej diya.");
        }

        // Provider ke bills
        public async Task<IEnumerable<BillListItemVM>> GetProviderBillsAsync(
            int providerId)
            => await _repo.GetProviderBillsAsync(providerId);

        // Hospital ke verify-pending bills
        public async Task<IEnumerable<BillListItemVM>> GetBillsForHospitalVerifyAsync(
            int hospitalId)
            => await _repo.GetBillsForHospitalVerifyAsync(hospitalId);

    }
  


    }