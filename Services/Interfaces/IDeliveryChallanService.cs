using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDeliveryChallanService
    {
        Task<DeliveryChallanVM> GetPickupForDelivery(int pickupId);
        Task<int> SaveDelivery(DeliveryChallanVM model);
        Task<List<DeliveryListVM>> GetDeliveryList();

        Task<List<DeliveryChallanItemVM>>
        GetDeliveryItems(int deliveryId);
    }
}
