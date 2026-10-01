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
                       ?? throw new Exception("The bill could not be loaded.");

            // 2. Agreement info
            var agr = await _repo.GetAgreementInfoAsync(hospitalId)
                      ?? throw new Exception(
                             "No active agreement found for this hospital.");

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
                    "A reason is required to override the score.", 0);

            // Duplicate check
            var existing = await _repo.GetBillByHospitalMonthAsync(
                vm.HospitalId, vm.BillingMonth, vm.BillingYear);

            if (existing != null &&
                existing.Status is not ("Draft" or "HospitalRejected" or "CMSRejected"))
                return (false,
                    $"A bill for this month is already in '{existing.Status}' " +
                    $"status.", 0);

            // Agreement, beds and rate come from the database, never from the form.
            var agr = await _repo.GetAgreementInfoByProviderHospitalAsync(
                vm.ProviderId, vm.HospitalId);
            if (agr == null)
                return (false,
                    "There is no active agreement between this provider and hospital.", 0);
            vm.AgreementId = agr.AgreementId;
            vm.SanctionedBeds = agr.SanctionedBeds;
            vm.RatePerBedPerYear = agr.RatePerBedPerYear;
            vm.GSTPercent = 18m;

            if (existing != null && existing.ProviderId != vm.ProviderId)
                return (false, "This bill is not yours.", 0);

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
                    Remarks = "Bill created"
                });
            }
            else
            {
                bill.Id = existing.Id;
                await _repo.UpdateBillAsync(bill);
                billId = existing.Id;
            }

            return (true, "Draft saved.", billId);
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
                return (false, "Bill not found.");
            if (bill.Status != "HospitalApproved")
                return (false,
                    "CMS can only act on bills in 'HospitalApproved' status.");
            if (!approve && string.IsNullOrWhiteSpace(remarks))
                return (false,
                    "A reason is required when rejecting.");

            string newStatus = approve ? "CMSApproved" : "CMSRejected";
            string logRemark = approve
                ? $"CMS approved. Net Payable: " +
                  $"₹{bill.NetPayableAmount:N2}"
                : $"CMS rejected. Reason: {remarks}";

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
                ? $"✅ Bill approved. Net Payable: " +
                  $"₹{bill.NetPayableAmount:N2}"
                : "❌ Bill rejected.");
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
                       ?? throw new Exception("The bill could not be loaded.");

            // 2. Agreement info (Provider + Hospital dono se)
            var agr = await _repo.GetAgreementInfoByProviderHospitalAsync(
                          providerId, hospitalId)
                      ?? throw new Exception(
                             "No active agreement found between this " +
                             "provider and hospital.");

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
                return (false, "Bill not found.");
            if (bill.ProviderId != providerId)
                return (false, "This bill is not yours.");
            if (bill.Status is not ("Draft" or "HospitalRejected" or "CMSRejected"))
                return (false,
                    $"Bill is in '{bill.Status}' status — it cannot be submitted.");
            if (bill.WPRAvgScore <= 0)
                return (false,
                    "The WPR score is 0. Check the score before submitting.");

            await _repo.UpdateBillStatusAsync(
                billId, "HospitalSubmitted", userId, null);

            await _repo.InsertWorkflowLogAsync(new BillWorkflowLog
            {
                BillId = billId,
                FromStatus = bill.Status,
                ToStatus = "HospitalSubmitted",
                ActionBy = userId,
                ActionAt = DateTime.Now,
                Remarks = "Provider submitted to Hospital"
            });

            return (true, "Bill sent to the hospital. ✅");
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
                return (false, "Bill not found.");
            if (bill.HospitalId != hospitalId)
                return (false, "This bill does not belong to your hospital.");
            if (bill.Status != "HospitalSubmitted")
                return (false,
                    "Only bills in 'HospitalSubmitted' status can be verified.");
            if (!approve && string.IsNullOrWhiteSpace(remarks))
                return (false,
                    "A reason is required when rejecting.");

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
                    ? $"Hospital verified. Net: ₹{bill.NetPayableAmount:N2}"
                    : $"Hospital rejected. Reason: {remarks}"
            });

            return (true, approve
                ? "✅ Bill verified — CMS will approve it next."
                : "❌ Bill rejected — sent back to the provider.");
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