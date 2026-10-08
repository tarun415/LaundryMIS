using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<LoginResult> Login(string username, string password, int roleId);
        // loginId is a lower-cased email or a 10-digit mobile number (see ContactRules.NormalizeLoginId)
        Task<LoginResult?> LoginHospital(string loginId, string password);
        Task<LoginResult?> LoginProvider(string loginId, string password);
        Task<LoginResult> LoginCms(int hospitalId, string password);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<LoginResult> RegisterHospital(RegisterVM model);
        Task<LoginResult> RegisterProvider(RegisterVM model);
    }
}