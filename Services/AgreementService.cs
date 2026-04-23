using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class AgreementService
    {
        private readonly AgreementRepository _repo;

        public AgreementService(AgreementRepository repo)
        {
            _repo = repo;
        }

        public async Task SaveAsync(AgreementVM model, string? filePath)
        {
            if (model.BedCount <= 0)
                throw new Exception("Invalid Bed Count");

            if (model.RatePerBed <= 0)
                throw new Exception("Invalid Rate");

            await _repo.InsertAsync(model, filePath);
        }
    }
}
