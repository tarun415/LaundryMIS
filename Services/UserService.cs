using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<LoginResult?> Login(string username, string password, int roleId)
        {
            return await _repo.Login(username, password, roleId);
        }
        //public async Task<User?> Login(string username, string password, int roleId)
        //{
        //    return await _repo.Login(username, password, roleId);
        //}

        public async Task<LoginResult?> LoginHospital(int? hospitalId, string password)
        {
            return await _repo.LoginHospital(hospitalId, password);
        }

        public async Task<LoginResult?> LoginProvider(int? providerId, string password)
        {
            return await _repo.LoginProvider(providerId, password);
        }
    }
}