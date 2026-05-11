using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<VerifyDeliveryVM> GetDeliveryByIdAsync(int entryId);

        Task<int> VerifyDeliveryAsync(VerifyDeliveryModel model);
    }
}
