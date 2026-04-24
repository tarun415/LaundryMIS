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
