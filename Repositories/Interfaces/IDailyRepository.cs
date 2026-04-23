using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDailyRepository
    {
        Task<int> InsertAsync(DailyEntryVM model);
        Task<List<DailyEntryVM>> GetAllEntries();
        Task<List<Hospital>> GetHospitalsByProvider(int providerId);

        Task UpdateStatus(int id, string status);

        Task<List<LinenType>> GetLinenTypes();

        Task<int> InsertDelivery(DeliveryVM model);

        Task<IEnumerable<dynamic>> GetPendingEntries(int providerId);
        Task<dynamic> GetEntryWithItems(int id);
    }
}