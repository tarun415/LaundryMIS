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
                return new ClaimResult { Message = isHospital ? "Select your hospital." : "Select your firm." };

            // Hospitals register without a code; vendors still need the one DGMH issued
            string? codeHash = null;
            if (!isHospital)
            {
                if (string.IsNullOrWhiteSpace(model.Code))
                    return new ClaimResult { Message = "Enter the activation code." };
                if (!ActivationCodeHelper.LooksValid(model.Code))
                    return new ClaimResult { BadCode = true, Message = "An activation code has 10 characters (for example K7M49-QXD2P)." };
                codeHash = ActivationCodeHelper.Hash(model.Code);
            }

            return await _repo.ClaimAsync(
                isHospital ? "Hospital" : "Provider",
                entityId.Value,
                codeHash,
                model.Phone,
                model.Email.Trim(),
                PasswordHasher.Hash(model.Password));
        }
    }
}
