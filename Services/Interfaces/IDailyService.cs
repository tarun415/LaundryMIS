using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDailyService
    {
        Task<int> SaveAsync(DailyEntryVM model);
        Task<List<DailyEntryListVM>> GetAllEntries();
        Task<List<DailyEntryItemsVM>> GetAllItems(int id);
        Task<List<Hospital>> GetHospitalsByProvider(int providerId);
        Task<List<WardVM>> GetWards();
        Task UpdateStatus(int id, string status);
        Task<List<LinenType>> GetLinenTypes();

        Task<dynamic> GetEntryForDelivery(int id);
        Task<IEnumerable<dynamic>> GetPendingEntries(int providerId);

        Task<int> DeliverAsync(DeliveryVM model);

        Task<List<DailyEntryListVM>> SearchDailyEntries(string status, int? hospitalId, int? wardId, DateTime? date);
    }
}
