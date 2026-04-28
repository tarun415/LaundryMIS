using LaudaryMis.Models;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> Login(string username, string password, int roleId);
        Task<User?> LoginHospital(int? hospitalId, string password);
        Task<User?> LoginProvider(int? providerId, string password);
    }
}