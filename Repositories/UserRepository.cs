using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User?> Login(string username, string password, int roleId)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName 
                        FROM Tbl_Users u
                        INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                        WHERE u.Username = @Username AND u.RoleId = @RoleId AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(sql, new
            {
                Username = username,
                RoleId = roleId
            });

            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }

        public async Task<User?> LoginHospital(int? hospitalId, string password)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName 
                        FROM Tbl_Users u
                        INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                        WHERE u.HospitalId = @HospitalId AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(sql, new
            {
                HospitalId = hospitalId
            });

            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }

        public async Task<User?> LoginProvider(int? providerId, string password)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName 
                        FROM Tbl_Users u
                        INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                        WHERE u.ProviderId = @ProviderId AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(sql, new
            {
                ProviderId = providerId
            });

            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }
    }
}