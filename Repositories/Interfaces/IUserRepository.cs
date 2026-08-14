using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<LoginResult> Login(string username, string password, int roleId);
        Task<LoginResult?> LoginHospital(int? hospitalId, string password);
        Task<LoginResult?> LoginProvider(int? providerId, string password);
    }
}