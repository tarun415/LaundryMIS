using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using LaudaryMis.Repositories.Interfaces;  // 🔥 ADD THIS

namespace LaudaryMis.Services
{
    public class AgreementService: IAgreementService
    {
        private readonly IAgreementRepository _repo;

        public AgreementService(IAgreementRepository repo)
        {
            _repo = repo;
        }


        //public async Task SaveAsync(AgreementVM model, string? filePath)
        //{
        //    if (model.BedCount <= 0)
        //        throw new Exception("Invalid Bed Count");

        //    if (model.RatePerBed <= 0)
        //        throw new Exception("Invalid Rate");

        //    await _repo.InsertAsync(model, filePath);
        //}
        public async Task SaveAsync(AgreementVM model, string? filePath)
        {
            if (model.BedCount <= 0)
                throw new Exception("Invalid Bed Count");

            if (model.RatePerBed <= 0)
                throw new Exception("Invalid Rate");
            if (model.Id > 0)
            {
                var existing = await _repo.GetByIdAsync(model.Id);

                if (filePath == null)
                    filePath = existing.FilePath;
            }
            await _repo.SaveAsync(model, filePath);
        }

        public async Task<IEnumerable<AgreementVM>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<AgreementVM> GetAgreementByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);

            if (data == null)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }

    }
}
