using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class DeliveryChallanService : IDeliveryChallanService
    {
        private readonly IDeliveryChallanRepository _repo;

        public DeliveryChallanService(
            IDeliveryChallanRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeliveryChallanVM> GetPickupForDelivery(int pickupId)
        {
            return await _repo.GetPickupForDelivery(pickupId);
        }

        public async Task<int> SaveDelivery(DeliveryChallanVM model)
        {
            return await _repo.SaveDelivery(model);
        }
        public async Task<List<DeliveryListVM>>
GetDeliveryList()
        {
            return await _repo.GetDeliveryList();
        }

        public async Task<List<DeliveryChallanItemVM>>
        GetDeliveryItems(int deliveryId)
        {
            return await _repo.GetDeliveryItems(deliveryId);
        }
    }
}