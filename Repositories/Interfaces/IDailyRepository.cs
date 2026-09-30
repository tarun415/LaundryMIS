using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDailyRepository
    {
        Task<int> InsertAsync(DailyEntryVM model);
        Task<List<DailyEntryListVM>> GetAllEntries(int? hospitalId = null, int? providerId = null);
        Task<List<DailyEntryItemsVM>> GetAllItems(int id, int? providerId = null);
        Task<List<Hospital>> GetHospitalsByProvider(int providerId);

        Task<List<WardVM>> GetWards();

        Task UpdateStatus(int id, string status);

        Task<List<LinenType>> GetLinenTypes();

        Task<int> InsertDelivery(DeliveryVM model);

        Task<IEnumerable<dynamic>> GetPendingEntries(int providerId);
        Task<dynamic> GetEntryWithItems(int id);
        Task<List<DailyEntryListVM>> SearchDailyEntries(string status, int? hospitalId, int? wardId, DateTime? date, int? providerId = null);
        Task<DailyEntryVM> GetDailyEntryByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsEntryOwnedByProviderAsync(int entryId, int providerId);
        Task<int> UpdateAsync(DailyEntryVM model);

     

    }
}