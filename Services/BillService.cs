using LaudaryMis.Helpers;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    // The monthly bill is no longer drafted by hand: it is worked out from the CMS-verified WPRs.
    public class BillService : IBillService
    {
        private readonly IBillRepository _repo;
        private readonly IWPRRepository _wprRepo;
        private readonly PortalCalendar _calendar;

        public BillService(IBillRepository repo, IWPRRepository wprRepo, PortalCalendar calendar)
        {
            _repo = repo;
            _wprRepo = wprRepo;
            _calendar = calendar;
        }

        public async Task<List<BillMonthVM>> GetMonthsAsync(int providerId)
        {
            var rows = (await _repo.GetMonthRowsAsync(providerId))
                .Where(r => !_calendar.IsBeforeStart(r.Month, r.Year))
                .ToList();
            var disputes = await _repo.GetDisputeCountsAsync(providerId);
            var prints = await _repo.GetPrintLogAsync(providerId);
            var agreements = new Dictionary<int, BillAgreementInfo?>();

            foreach (var r in rows)
            {
                r.ExpectedWeeks = WprParameters.WeeksInMonth(r.Month, r.Year);

                var d = disputes.FirstOrDefault(x =>
                    x.HospitalId == r.HospitalId && x.BillMonth == r.Month && x.BillYear == r.Year);
                r.Version = 1 + (d?.Resolved ?? 0);
                r.HasOpenDispute = (d?.Opened ?? 0) > 0;

                var mine = prints.Where(p =>
                    p.HospitalId == r.HospitalId && p.BillMonth == r.Month && p.BillYear == r.Year).ToList();
                r.CurrentVersionPrinted = mine.Any(p => p.Version == r.Version);
                r.LastPrintedAt = mine.Select(p => (DateTime?)p.PrintedAt).Max();

                if (r.IsReady && r.AvgScore.HasValue)
                {
                    if (!agreements.TryGetValue(r.HospitalId, out var agr))
                        agreements[r.HospitalId] = agr = await _repo.GetAgreementInfoAsync(providerId, r.HospitalId);

                    if (agr != null)
                        r.NetPayableAmount = Compute(agr, r.AvgScore.Value).NetPayableAmount;
                }
            }

            return rows;
        }

        public async Task<(BillVM? Bill, string Message)> GetBillAsync(
            int providerId, int hospitalId, int month, int year)
        {
            if (month is < 1 or > 12 || year < 2000)
                return (null, "Select a valid month.");

            if (_calendar.IsBeforeStart(month, year))
                return (null, _calendar.BeforeStartMessage("Bill"));

            var row = (await _repo.GetMonthRowsAsync(providerId, hospitalId, month, year)).FirstOrDefault();
            if (row == null)
                return (null, "There is no WPR for this month.");

            int expected = WprParameters.WeeksInMonth(month, year);
            if (row.PendingWeeks > 0 || row.VerifiedWeeks < expected || !row.AvgScore.HasValue)
                return (null,
                    $"The bill is available once all {expected} weeks of {row.MonthLabel} are verified by the CMS " +
                    $"({row.VerifiedWeeks} of {expected} verified so far).");

            var agr = await _repo.GetAgreementInfoAsync(providerId, hospitalId);
            if (agr == null)
                return (null, "There is no active agreement between you and this hospital.");

            var bill = Compute(agr, row.AvgScore.Value);
            bill.BillingMonth = month;
            bill.BillingYear = year;
            bill.WPRWeeksFound = row.VerifiedWeeks;

            var disputes = (await _repo.GetDisputeCountsAsync(providerId)).FirstOrDefault(x =>
                x.HospitalId == hospitalId && x.BillMonth == month && x.BillYear == year);
            bill.Version = 1 + (disputes?.Resolved ?? 0);

            bill.Weeks = (await _wprRepo.GetWprListAsync(hospitalId, providerId, WprStatus.Verified, month, year))
                .OrderBy(w => w.Week).ToList();

            return (bill, "");
        }

        public Task LogPrintAsync(BillVM bill, int userId) =>
            _repo.InsertPrintLogAsync(bill.ProviderId, bill.HospitalId, bill.BillingMonth, bill.BillingYear,
                bill.Version, bill.WPRAvgScore, bill.NetPayableAmount, userId);

        public Task<List<BillPrintLogVM>> GetPrintLogAsync(int providerId) => _repo.GetPrintLogAsync(providerId);

        public async Task<List<PrintedMonthVM>> GetDisputableMonthsAsync(int providerId, int hospitalId, int year)
        {
            var months = (await GetMonthsAsync(providerId))
                .Where(m => m.HospitalId == hospitalId && m.Year == year
                            && m.IsReady && m.CurrentVersionPrinted && !m.HasOpenDispute)
                .OrderBy(m => m.Month)
                .Select(m => new PrintedMonthVM
                {
                    Month = m.Month,
                    Label = new DateTime(m.Year, m.Month, 1).ToString("MMMM")
                })
                .ToList();

            return months;
        }

        public async Task<List<OptionVM>> GetHospitalsAsync(int providerId)
        {
            var rows = await _repo.GetMonthRowsAsync(providerId);
            return rows
                .GroupBy(r => r.HospitalId)
                .Select(g => new OptionVM { Id = g.Key, Name = g.First().HospitalName })
                .OrderBy(o => o.Name)
                .ToList();
        }

        // Contract (Part-III, Payment Mechanism):
        //   monthly gross = beds x rate per bed per year / 12 (ex-GST); the WPR band % applies to it;
        //   GST (18%) is added on the base payable and TDS (2%, on the base payable) is deducted.
        private static BillVM Compute(BillAgreementInfo agr, decimal avgScore)
        {
            var bill = new BillVM
            {
                AgreementId = agr.AgreementId,
                HospitalId = agr.HospitalId,
                ProviderId = agr.ProviderId,
                HospitalName = agr.HospitalName,
                District = agr.District,
                ProviderName = agr.ProviderName,
                ContractNo = agr.ContractNo,
                SanctionedBeds = agr.SanctionedBeds,
                RatePerBedPerYear = agr.RatePerBedPerYear,
                GSTPercent = 18m,
                WPRAvgScore = Math.Round(avgScore, 2, MidpointRounding.AwayFromZero)
            };

            bill.AnnualValueExGST = bill.SanctionedBeds * bill.RatePerBedPerYear;
            bill.AnnualValueInGST = Round2(bill.AnnualValueExGST * (1 + bill.GSTPercent / 100));
            bill.MonthlyGrossAmount = Round2(bill.AnnualValueExGST / 12);
            bill.PaymentBandPercent = PaymentBand(bill.WPRAvgScore);
            bill.BasePayableAmount = Round2(bill.MonthlyGrossAmount * bill.PaymentBandPercent / 100);
            bill.GSTAmount = Round2(bill.BasePayableAmount * bill.GSTPercent / 100);
            bill.TDSAmount = Round2(bill.BasePayableAmount * 0.02m);
            bill.NetPayableAmount = bill.BasePayableAmount + bill.GSTAmount - bill.TDSAmount;
            return bill;
        }

        // 0-20 nil, 21-40 40%, 41-60 60%, 61-70 80%, 71-80 90%, 81-100 100%
        private static decimal PaymentBand(decimal avgScore) => avgScore switch
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
    }
}
