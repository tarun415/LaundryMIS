using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
namespace LaudaryMis.Services
{

    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _repository;

        public DeliveryService(IDeliveryRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<MonthlyVerificationListVM>>
      GetWeeklyVerificationAsync(
          int hospitalId,
          int month,
          int year)
        {
            return await _repository
                .GetWeeklyVerificationAsync(
                    hospitalId,
                    month,
                    year);
        }

        public async Task<List<MonthlyVerificationListVM>>
            GetWeeklyDrillDownAsync(
                int hospitalId,
                int month,
                int year,
                int weekNo)
        {
            return await _repository
                .GetWeeklyDrillDownAsync(
                    hospitalId,
                    month,
                    year,
                    weekNo);
        }

        public async Task<int> SaveWeeklyVerificationLogAsync(
       WeeklyVerificationModel model)
        {
            return await _repository
                .SaveWeeklyVerificationLogAsync(model);
        }



        public async Task<int> SaveMonthlyLogBookAsync(
     WeeklyVerificationModel model)
        {
            return await _repository
                .SaveMonthlyLogBookAsync(model);
        }


    }
}
