using LaudaryMis.Helpers;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class ActivationCodeService : IActivationCodeService
    {
        private readonly IActivationCodeRepository _repo;

        public ActivationCodeService(IActivationCodeRepository repo)
        {
            _repo = repo;
        }

        public Task<List<ActivationRowVM>> GetStatusAsync(string entityType) =>
            _repo.GetStatusAsync(entityType);

        public Task<List<IssuedCodeVM>> IssueAsync(
            string entityType, IReadOnlyCollection<int> entityIds, int adminUserId) =>
            _repo.IssueAsync(entityType, entityIds, ActivationCodeHelper.Generate, adminUserId);

        public Task<List<NameOption>> GetUnclaimedHospitalsAsync(int districtId) =>
            _repo.GetUnclaimedHospitalsAsync(districtId);

        public Task<List<NameOption>> GetUnclaimedProvidersAsync() =>
            _repo.GetUnclaimedProvidersAsync();

        public async Task<ClaimResult> ClaimAsync(ClaimVM model)
        {
            bool isHospital = model.RoleId == 2;
            int? entityId = isHospital ? model.HospitalId : model.ProviderId;

            if (entityId is null or <= 0)
                return new ClaimResult { Message = isHospital ? "Apna hospital chunein." : "Apni firm chunein." };

            if (!ActivationCodeHelper.LooksValid(model.Code))
                return new ClaimResult { BadCode = true, Message = "Activation code 10 characters ka hota hai (jaise K7M49-QXD2P)." };

            return await _repo.ClaimAsync(
                isHospital ? "Hospital" : "Provider",
                entityId.Value,
                ActivationCodeHelper.Hash(model.Code),
                model.Phone,
                model.Email.Trim(),
                PasswordHasher.Hash(model.Password));
        }
    }
}
