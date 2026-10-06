using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IReportRepository
    {      
        Task<List<DeliverySummaryVM>> GetDeliverySummaryReport();

        Task<List<DeliveryHistoryVM>>
            GetDeliveryHistory(int pickupId);

        // hospitalId / providerId limit the totals to one hospital or one vendor (null = everything)
        Task<List<WeeklyDeliveryReport>> WeeklyDeliveryReport(
    DateTime fromDate,
    DateTime toDate,
    int? hospitalId = null,
    int? providerId = null);

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



        Task<DeliveryAgingSummaryVM>
          GetDeliveryAgingSummary();

        Task<List<DeliveryAgingReportVM>>
            GetDeliveryAgingReport();

        Task<List<DeliveryAgingItemVM>>
            GetDeliveryAgingDetailItems(int pickupId);
    }
}