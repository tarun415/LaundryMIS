using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDailyRepository
    {
        Task<int> InsertAsync(DailyEntryVM model);
        Task<List<DailyEntryListVM>> GetAllEntries();
        Task<List<DailyEntryItemsVM>> GetAllItems(int id);
        Task<List<Hospital>> GetHospitalsByProvider(int providerId);

        Task<List<WardVM>> GetWards();

        Task UpdateStatus(int id, string status);

        Task<List<LinenType>> GetLinenTypes();

        Task<int> InsertDelivery(DeliveryVM model);

        Task<IEnumerable<dynamic>> GetPendingEntries(int providerId);
        Task<dynamic> GetEntryWithItems(int id);
        Task<List<DailyEntryListVM>> SearchDailyEntries(string status, int? hospitalId, int? wardId, DateTime? date);

    }
}