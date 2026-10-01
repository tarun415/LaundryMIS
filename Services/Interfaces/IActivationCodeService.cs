using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IActivationCodeService
    {
        Task<List<ActivationRowVM>> GetStatusAsync(string entityType);
        Task<List<IssuedCodeVM>> IssueAsync(string entityType, IReadOnlyCollection<int> entityIds, int adminUserId);
        Task<List<NameOption>> GetUnclaimedHospitalsAsync(int districtId);
        Task<List<NameOption>> GetUnclaimedProvidersAsync();
        Task<ClaimResult> ClaimAsync(ClaimVM model);
    }
}
