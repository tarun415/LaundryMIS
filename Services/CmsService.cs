using LaudaryMis.Helpers;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class CmsService : ICmsService
    {
        private readonly ICmsRepository _repo;

        public CmsService(ICmsRepository repo) => _repo = repo;

        public Task<List<OptionVM>> GetLoginDistrictsAsync() => _repo.GetLoginDistrictsAsync();

        public Task<List<OptionVM>> GetLoginHospitalsAsync(int districtId) => _repo.GetLoginHospitalsAsync(districtId);

        public Task<List<CmsAccountVM>> GetAccountsAsync(int? districtId) => _repo.GetAccountsAsync(districtId);

        public Task<List<OptionVM>> GetDistrictsAsync() => _repo.GetDistrictsAsync();

        public async Task<(bool Success, string PasswordOrMessage)> GeneratePasswordAsync(int hospitalId)
        {
            var password = PasswordGenerator.Generate();
            var ok = await _repo.SetPasswordAsync(hospitalId, PasswordHasher.Hash(password));
            return ok ? (true, password) : (false, "Hospital not found.");
        }

        public Task<bool> SetActiveAsync(int hospitalId, bool isActive) => _repo.SetActiveAsync(hospitalId, isActive);
    }
}
