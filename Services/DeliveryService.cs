using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using System.IO;
namespace LaudaryMis.Services
{

    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _repository;

        public DeliveryService(IDeliveryRepository repository)
        {
            _repository = repository;
        }

        public async Task<VerifyDeliveryVM> GetDeliveryByIdAsync(int entryId)
        {
            return await _repository.GetDeliveryByIdAsync(entryId);
        }

        public async Task<bool> VerifyDeliveryAsync(
            VerifyDeliveryVM model,
            int userId,
            string uploadPath)
        {
            if (model.LogBookFile == null ||
                model.LogBookFile.Length == 0)
            {
                return false;
            }

            // Extension Validation
            var allowed = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            string ext = Path.GetExtension(model.LogBookFile.FileName)
                             .ToLower();

            if (!allowed.Contains(ext))
            {
                return false;
            }

            // Create Folder
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Unique File Name
            string fileName =
                Guid.NewGuid() + ext;

            string fullPath =
                Path.Combine(uploadPath, fileName);

            // Save File
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.LogBookFile.CopyToAsync(stream);
            }

            var data = new VerifyDeliveryModel
            {
                DeliveryId = model.DeliveryId,
                VerifiedQty = model.VerifiedQty,
                LogBookPath = "/Uploads/LogBooks/" + fileName,
                VerifiedBy = userId
            };

            int result =
                await _repository.VerifyDeliveryAsync(data);

            return result > 0;
        }
    }
}
