using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDailyService
    {
        Task<int> SaveAsync(DailyEntryVM model);
        Task<List<DailyEntryVM>> GetAllEntries();
        Task<List<Hospital>> GetHospitalsByProvider(int providerId);
        Task UpdateStatus(int id, string status);
        Task<List<LinenType>> GetLinenTypes();

        Task<dynamic> GetEntryForDelivery(int id);
        Task<IEnumerable<dynamic>> GetPendingEntries(int providerId);

        Task<int> DeliverAsync(DeliveryVM model);
    }
}
