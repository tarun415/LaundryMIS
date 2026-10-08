using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface ICmsService
    {
        Task<List<OptionVM>> GetLoginDistrictsAsync();
        Task<List<OptionVM>> GetLoginHospitalsAsync(int districtId);

        Task<List<CmsAccountVM>> GetAccountsAsync(int? districtId);
        Task<List<OptionVM>> GetDistrictsAsync();

        // Returns the new plain password (shown to the admin once), or an error
        Task<(bool Success, string PasswordOrMessage)> GeneratePasswordAsync(int hospitalId);
        Task<bool> SetActiveAsync(int hospitalId, bool isActive);
    }
}
