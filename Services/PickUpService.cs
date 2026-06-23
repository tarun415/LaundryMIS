using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class PickUpService:IPickUpService
    {
        private readonly IPickUpRepository _repo;
        public PickUpService(IPickUpRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> SavePickup(PickupVM model)
        {
            if (model.Items == null || !model.Items.Any())
                throw new Exception("Pickup items required");

            return await _repo.SavePickup(model);
        }
        public async Task<List<PickupListVM>> GetPickupList()
        {
            return await _repo.GetPickupList();
        }
        public async Task<List<PickupItemListVM>> GetPickupItems(int pickupId)
        {
            return await _repo.GetPickupItems(pickupId);
        }
        public async Task<List<PickupListVM>> SearchPickupList(
          string status,
          int? hospitalId,
          int? wardId,
          DateTime? date)
        {
            return await _repo.SearchPickupList(
                status,
                hospitalId,
                wardId,
                date);
        }
        public async Task<DbResponse> DeletePickup(int pickupId)
        {
            return await _repo.DeletePickup(pickupId);
        }

        public async Task<PickupVM> GetPickupById(int pickupId)
        {
            return await _repo.GetPickupById(pickupId);
        }
        public async Task UpdatePrintUrl(
      int pickupId,
      string path)
        {
            await _repo.UpdatePrintUrl(
                pickupId,
                path);
        }
        public async Task<PickupVM> GetPickupForAcceptance(int pickupId)
        {
            return await _repo
                .GetPickupForAcceptance(pickupId);
        }
        public async Task<int> AcceptPickup(
    int pickupId,
    int userId,
    string remarks)
        {
            return await _repo
                .AcceptPickup(
                    pickupId,
                    userId,
                    remarks);
        }

        //    public async Task<int> AcceptDelivery(
        //int PickupId,
        //int userId,
        //string remarks)
        //    {
        //        return await _repo.AcceptDelivery(
        //            PickupId,
        //            userId,
        //            remarks);
        //    }
        public async Task<int> AcceptDelivery(
        int DeliveryId,
        int userId,
        string remarks)
        {
            return await _repo.AcceptDelivery(
                DeliveryId,
                userId,
                remarks);
        }
        public async Task<List<PickupDeliveryHistoryVM>>
GetPickupDeliveryHistory(int pickupId)
        {
            return await _repo
                .GetPickupDeliveryHistory(pickupId);
        }
        public async Task<int> VerifyDeliveries(
    int pickupId,
    string deliveryIds,
    int userId,
    string remarks)
        {
            return await _repo.VerifyDeliveries(
                pickupId,
                deliveryIds,
                userId,
                remarks);
        }
    }
}
