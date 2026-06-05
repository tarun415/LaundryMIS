using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDeliveryChallanRepository
    {
        Task<DeliveryChallanVM> GetPickupForDelivery(int pickupId);

        Task<int> SaveDelivery(DeliveryChallanVM model);

        Task<List<DeliveryListVM>> GetDeliveryList();

        Task<List<DeliveryChallanItemVM>> GetDeliveryItems(int deliveryId);
    }
}