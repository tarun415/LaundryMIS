using LaudaryMis.Models;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> Login(string RoleName, string password, int roleId);

    }
}
