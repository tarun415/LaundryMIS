using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResult?> Login(string username, string password, int roleId);
        Task<LoginResult?> LoginHospital(string loginId, string password);
        Task<LoginResult?> LoginProvider(string loginId, string password);
        Task<LoginResult?> LoginCms(int hospitalId, string password);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<LoginResult> RegisterHospital(RegisterVM model);
        Task<LoginResult> RegisterProvider(RegisterVM model);
    }
}