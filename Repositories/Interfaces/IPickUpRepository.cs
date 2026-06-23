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

        Task<PickupVM> GetPickupForAcceptance(int pickupId);

        Task<int> AcceptPickup(
            int pickupId,
            int userId,
            string remarks);

        //    Task<int> AcceptDelivery(
        //int PickupId,
        //int userId,
        //string remarks);
        Task<int> AcceptDelivery(
        int DeliveryId,
        int userId,
        string remarks);

        Task<List<PickupDeliveryHistoryVM>> GetPickupDeliveryHistory(int pickupId);

        Task<int> VerifyDeliveries(
    int pickupId,
    string deliveryIds,
    int userId,
    string remarks);
    }
}
