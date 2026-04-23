using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;

namespace LaudaryMis.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<User?> Login(string RoleName, string password, int roleId)
        {
            return await _repo.Login(RoleName, password, roleId);
        }
    }
}
