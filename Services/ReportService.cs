using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }      
        public async Task<List<DeliverySummaryVM>>
GetDeliverySummaryReport()
        {
            return await _repo
                .GetDeliverySummaryReport();
        }

        public async Task<List<DeliveryHistoryVM>>
        GetDeliveryHistory(int pickupId)
        {
            return await _repo
                .GetDeliveryHistory(pickupId);
        }
        public async Task<List<WeeklyDeliveryReport>> WeeklyDeliveryReport(
    DateTime fromDate,
    DateTime toDate)
        {
            return await _repo.WeeklyDeliveryReport(
                fromDate,
                toDate);
        }
        public async Task<List<MonthlyReportVM>>
GetMonthlyReport(
int year,
int month)
        {
            return await
                _repo.GetMonthlyReport(
                    year,
                    month);
        }
        public async Task<List<MonthlyPickupDetailVM>>
GetMonthlyPickupDetails(
int month,
int year)
        {
            return await
                _repo.GetMonthlyPickupDetails(
                    month,
                    year);
        }
        public async Task<List<PendingLinenReportVM>>
GetPendingLinenReport()
        {
            return await _repo.GetPendingLinenReport();
        }
        public async Task<DeliveryAgingReportPageVM>
    GetDeliveryAgingReport()
        {
            var summary =
                await _repo.GetDeliveryAgingSummary();

            var details =
                await _repo.GetDeliveryAgingReport();

            // If there is no outstanding linen (Pending <= 0), the pickup is
            // fully returned and should never be flagged as Critical/Warning
            // purely because the collection date is old.
            foreach (var item in details)
            {
                if (item.PendingQty <= 0)
                {
                    item.AgingStatus = "Normal";
                }
            }

            // Keep the summary tiles consistent with the adjusted statuses.
            if (summary != null)
            {
                summary.CriticalCount =
                    details.Count(x => x.AgingStatus == "Critical");
                summary.WarningCount =
                    details.Count(x => x.AgingStatus == "Warning");
                summary.NormalCount =
                    details.Count(x => x.AgingStatus == "Normal");
                summary.TotalPendingPickups =
                    details.Count(x => x.PendingQty > 0);
                summary.TotalPendingQty =
                    details.Where(x => x.PendingQty > 0)
                           .Sum(x => x.PendingQty);
            }

            return new DeliveryAgingReportPageVM
            {
                Summary = summary,
                Details = details
            };
        }

        public async Task<List<DeliveryAgingItemVM>>
        GetDeliveryAgingDetailItems(int pickupId)
        {
            return await _repo
                .GetDeliveryAgingDetailItems(pickupId);
        }
    }
}