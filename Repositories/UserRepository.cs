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

        public async Task<User?> Login(string RoleName, string password, int roleId)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName FROM Tbl_Users u 
                        INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId WHERE r.RoleName = @RoleName 
                        AND u.RoleId = @RoleId AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(sql, new
            {
                RoleName = RoleName,
                RoleId = roleId
            });

            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }
    }
}
