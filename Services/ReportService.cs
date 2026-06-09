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
    }
}