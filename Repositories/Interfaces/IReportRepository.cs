using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IReportRepository
    {      
        Task<List<DeliverySummaryVM>> GetDeliverySummaryReport();

        Task<List<DeliveryHistoryVM>>
            GetDeliveryHistory(int pickupId);

        Task<List<WeeklyDeliveryReport>> WeeklyDeliveryReport(
    DateTime fromDate,
    DateTime toDate);

        Task<List<MonthlyReportVM>>
GetMonthlyReport(
int year,
int month);
        Task<List<MonthlyPickupDetailVM>>
GetMonthlyPickupDetails(
int month,
int year);
    }
}