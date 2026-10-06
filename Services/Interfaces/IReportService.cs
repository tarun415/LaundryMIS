using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<DeliverySummaryVM>>GetDeliverySummaryReport();

        Task<List<DeliveryHistoryVM>> GetDeliveryHistory(int pickupId);

        Task<List<WeeklyDeliveryReport>> WeeklyDeliveryReport(DateTime fromDate,  DateTime toDate, int? hospitalId = null, int? providerId = null);

        Task<List<MonthlyReportVM>>
GetMonthlyReport(
int year,
int month,
int? hospitalId = null,
int? providerId = null);
        Task<List<MonthlyPickupDetailVM>>
GetMonthlyPickupDetails(
int month,
int year); 
        Task<List<PendingLinenReportVM>>
GetPendingLinenReport();
        Task<DeliveryAgingReportPageVM>
               GetDeliveryAgingReport(ISet<int>? visiblePickupIds = null);

        Task<List<DeliveryAgingItemVM>>
            GetDeliveryAgingDetailItems(int pickupId);
    }
}

