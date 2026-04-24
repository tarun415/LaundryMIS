using LaudaryMis.Models;
using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class WPRService: IWPRService
    {
        private readonly IWPRRepository _repo;
        public WPRService(IWPRRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> SaveAsync(WPRVM model, int hospitalId)
        {
            if (model.Details == null || !model.Details.Any())
                throw new Exception("No data");

            return await _repo.InsertAsync(model, hospitalId);
        }
        public async Task<List<WPRParameter>> GetParameters()
        {
            return (await _repo.GetParameters()).ToList();
        }

        public async Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId)
        {
            return (await _repo.GetHospitalAgreements(hospitalId)).ToList();
        }
    }
}
