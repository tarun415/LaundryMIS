using Dapper;
using LaudaryMis.Helpers;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
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
        public async Task<LoginResult> Login(
    string username,
    string password,
    int roleId)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName
                FROM Tbl_Users u
                INNER JOIN Tbl_Roles r 
                    ON u.RoleId = r.RoleId
                WHERE u.Username = @Username
                  AND u.RoleId = @RoleId
                  AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(
                sql,
                new
                {
                    Username = username,
                    RoleId = roleId
                });

            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid username."
                };
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Incorrect password."
                };
            }

            if (PasswordHasher.NeedsRehash(user.PasswordHash))
            {
                await con.ExecuteAsync(
                    "UPDATE Tbl_Users SET PasswordHash = @Hash WHERE UserId = @UserId",
                    new { Hash = PasswordHasher.Hash(password), user.UserId });
            }

            return new LoginResult
            {
                Success = true,
                User = user
            };
        }
    
        public async Task<LoginResult> LoginHospital(int? hospitalId, string password)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName
                FROM Tbl_Users u
                INNER JOIN Tbl_Roles r
                    ON u.RoleId = r.RoleId
                WHERE u.HospitalId = @HospitalId
                  AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(
                sql,
                new
                {
                    HospitalId = hospitalId
                });

            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid District or hospital."
                };
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Incorrect password."
                };
            }

            if (PasswordHasher.NeedsRehash(user.PasswordHash))
            {
                await con.ExecuteAsync(
                    "UPDATE Tbl_Users SET PasswordHash = @Hash WHERE UserId = @UserId",
                    new { Hash = PasswordHasher.Hash(password), user.UserId });
            }

            return new LoginResult
            {
                Success = true,
                User = user
            };
        }
        public async Task<LoginResult> LoginProvider(int? providerId, string password)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var sql = @"SELECT u.*, r.RoleName
                FROM Tbl_Users u
                INNER JOIN Tbl_Roles r
                    ON u.RoleId = r.RoleId
                WHERE u.ProviderId = @ProviderId
                  AND u.IsActive = 1";

            var user = await con.QueryFirstOrDefaultAsync<User>(
                sql,
                new
                {
                    ProviderId = providerId
                });

            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid provider."
                };
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Incorrect password."
                };
            }

            if (PasswordHasher.NeedsRehash(user.PasswordHash))
            {
                await con.ExecuteAsync(
                    "UPDATE Tbl_Users SET PasswordHash = @Hash WHERE UserId = @UserId",
                    new { Hash = PasswordHasher.Hash(password), user.UserId });
            }

            return new LoginResult
            {
                Success = true,
                User = user
            };
        }


    }
}