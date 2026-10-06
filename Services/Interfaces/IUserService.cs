using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResult?> Login(string username, string password, int roleId);
        Task<LoginResult?> LoginHospital(string loginId, string password);
        Task<LoginResult?> LoginProvider(string loginId, string password);
        Task<LoginResult> RegisterHospital(RegisterVM model);
        Task<LoginResult> RegisterProvider(RegisterVM model);
    }
}