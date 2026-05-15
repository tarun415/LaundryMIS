using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDeliveryService
    {


        Task<List<MonthlyVerificationListVM>>
    GetWeeklyVerificationAsync(
        int hospitalId,
        int month,
        int year);

        Task<List<MonthlyVerificationListVM>>
            GetWeeklyDrillDownAsync(
                int hospitalId,
                int month,
                int year,
                int weekNo);


        Task<int> SaveWeeklyVerificationLogAsync(
        WeeklyVerificationModel model);

        Task<int> SaveMonthlyLogBookAsync(
      WeeklyVerificationModel model);


    }
}
