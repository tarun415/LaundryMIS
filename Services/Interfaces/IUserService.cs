using LaudaryMis.Models;

namespace LaudaryMis.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> Login(string username, string password, int roleId);
        Task<User?> LoginHospital(int? hospitalId, string password);
        Task<User?> LoginProvider(int? providerId, string password);
    }
}