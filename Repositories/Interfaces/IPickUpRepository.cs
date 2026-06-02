using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IPickUpRepository
    {
        Task<int> SavePickup(PickupVM model);
        Task<List<PickupListVM>> GetPickupList();

        Task<List<PickupItemListVM>> GetPickupItems(int pickupId);

        Task<List<PickupListVM>> SearchPickupList(
            string status,
            int? hospitalId,
            int? wardId,
            DateTime? date);

        Task<DbResponse> DeletePickup(int pickupId);

        Task<PickupVM> GetPickupById(int pickupId);

        Task UpdatePrintUrl(
 int pickupId,
 string path);
    }
}
