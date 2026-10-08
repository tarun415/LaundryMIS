using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface ICmsRepository
    {
        // Sign-in dropdowns (only districts / hospitals that have an active CMS login)
        Task<List<OptionVM>> GetLoginDistrictsAsync();
        Task<List<OptionVM>> GetLoginHospitalsAsync(int districtId);

        // Admin: every hospital with the state of its CMS login
        Task<List<CmsAccountVM>> GetAccountsAsync(int? districtId);
        Task<List<OptionVM>> GetDistrictsAsync();

        // Creates the hospital's CMS login if it has none, otherwise replaces its password.
        // The user must change it at the next sign-in. False when the hospital does not exist.
        Task<bool> SetPasswordAsync(int hospitalId, string passwordHash);
        Task<bool> SetActiveAsync(int hospitalId, bool isActive);
    }
}
