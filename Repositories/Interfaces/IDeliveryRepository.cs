using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDeliveryRepository
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

        Task<int> SaveWeeklyVerificationLogAsync(WeeklyVerificationModel model);

        Task<int> SaveMonthlyLogBookAsync(
      WeeklyVerificationModel model);


    }
}
