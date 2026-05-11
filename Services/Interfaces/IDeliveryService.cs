using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IDeliveryService
    {
        Task<VerifyDeliveryVM> GetDeliveryByIdAsync(int entryId);

        Task<bool> VerifyDeliveryAsync(
            VerifyDeliveryVM model,
            int userId,
            string uploadPath);
    }
}
