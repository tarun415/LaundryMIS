using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IActivationCodeRepository
    {
        Task<List<ActivationRowVM>> GetStatusAsync(string entityType);

        // Stores the new code hashes (replacing any unused code) and returns who was covered.
        // Entities that already have a login are skipped.
        Task<List<IssuedCodeVM>> IssueAsync(
            string entityType, IReadOnlyCollection<int> entityIds,
            Func<string> newCode, int adminUserId);

        Task<List<NameOption>> GetUnclaimedHospitalsAsync(int districtId);
        Task<List<NameOption>> GetUnclaimedProvidersAsync();

        Task<ClaimResult> ClaimAsync(
            string entityType, int entityId, string codeHash,
            string phone, string email, string passwordHash);
    }
}
