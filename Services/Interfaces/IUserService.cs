using LaudaryMis.Models;

namespace LaudaryMis.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> Login(string email, string password, int roleId);

    }
}
