using Dapper;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Repositories
{
    public class CmsRepository : ICmsRepository
    {
        private readonly string _connStr;

        public CmsRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private SqlConnection Conn() => new(_connStr);

        public async Task<List<OptionVM>> GetLoginDistrictsAsync()
        {
            using var con = Conn();
            return (await con.QueryAsync<OptionVM>(@"
                SELECT DISTINCT d.DistrictID AS Id, d.DistrictName AS Name
                FROM Tbl_Users u
                JOIN tbl_Hospitals h ON h.HospitalId = u.HospitalId
                JOIN DistrictMaster d ON d.DistrictID = h.DistrictId
                WHERE u.RoleId = 4 AND u.IsActive = 1 AND h.IsActive = 1
                ORDER BY d.DistrictName")).ToList();
        }

        public async Task<List<OptionVM>> GetLoginHospitalsAsync(int districtId)
        {
            using var con = Conn();
            return (await con.QueryAsync<OptionVM>(@"
                SELECT h.HospitalId AS Id, h.HospitalName AS Name
                FROM Tbl_Users u
                JOIN tbl_Hospitals h ON h.HospitalId = u.HospitalId
                WHERE u.RoleId = 4 AND u.IsActive = 1 AND h.IsActive = 1
                  AND h.DistrictId = @districtId
                ORDER BY h.HospitalName", new { districtId })).ToList();
        }

        public async Task<List<OptionVM>> GetDistrictsAsync()
        {
            using var con = Conn();
            return (await con.QueryAsync<OptionVM>(@"
                SELECT DISTINCT d.DistrictID AS Id, d.DistrictName AS Name
                FROM tbl_Hospitals h
                JOIN DistrictMaster d ON d.DistrictID = h.DistrictId
                WHERE h.IsActive = 1
                ORDER BY d.DistrictName")).ToList();
        }

        public async Task<List<CmsAccountVM>> GetAccountsAsync(int? districtId)
        {
            using var con = Conn();
            return (await con.QueryAsync<CmsAccountVM>(@"
                SELECT h.HospitalId, h.HospitalName, h.DistrictId, d.DistrictName,
                       CAST(CASE WHEN u.UserId IS NULL THEN 0 ELSE 1 END AS BIT) AS HasAccount,
                       CAST(ISNULL(u.IsActive, 0) AS BIT)           AS IsActive,
                       CAST(ISNULL(u.MustChangePassword, 0) AS BIT) AS MustChangePassword
                FROM tbl_Hospitals h
                LEFT JOIN DistrictMaster d ON d.DistrictID = h.DistrictId
                LEFT JOIN Tbl_Users u ON u.HospitalId = h.HospitalId AND u.RoleId = 4
                WHERE h.IsActive = 1
                  AND (@districtId IS NULL OR h.DistrictId = @districtId)
                ORDER BY d.DistrictName, h.HospitalName", new { districtId })).ToList();
        }

        public async Task<bool> SetPasswordAsync(int hospitalId, string passwordHash)
        {
            using var con = Conn();
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            var hospitalName = await con.ExecuteScalarAsync<string?>(
                "SELECT HospitalName FROM tbl_Hospitals WHERE HospitalId = @hospitalId",
                new { hospitalId }, tran);
            if (hospitalName == null)
            {
                tran.Rollback();
                return false;
            }

            var userId = await con.ExecuteScalarAsync<int?>(
                "SELECT UserId FROM Tbl_Users WHERE RoleId = 4 AND HospitalId = @hospitalId",
                new { hospitalId }, tran);

            if (userId == null)
            {
                // Email is unique on Tbl_Users and the CMS never uses it to sign in
                await con.ExecuteAsync(@"
                    INSERT INTO Tbl_Users
                        (ProviderId, FullName, Email, PasswordHash, RoleId, HospitalId, IsActive, Username, MustChangePassword)
                    VALUES
                        (NULL, @FullName, @Email, @PasswordHash, 4, @hospitalId, 1, @Username, 1)",
                    new
                    {
                        FullName = "CMS - " + hospitalName.Trim(),
                        Email = $"cms-h{hospitalId}@cms.laundrymis.local",
                        PasswordHash = passwordHash,
                        hospitalId,
                        Username = $"cms-h{hospitalId}"
                    }, tran);
            }
            else
            {
                await con.ExecuteAsync(@"
                    UPDATE Tbl_Users
                    SET PasswordHash = @PasswordHash, MustChangePassword = 1, IsActive = 1
                    WHERE UserId = @userId",
                    new { PasswordHash = passwordHash, userId }, tran);
            }

            tran.Commit();
            return true;
        }

        public async Task<bool> SetActiveAsync(int hospitalId, bool isActive)
        {
            using var con = Conn();
            return await con.ExecuteAsync(
                "UPDATE Tbl_Users SET IsActive = @isActive WHERE RoleId = 4 AND HospitalId = @hospitalId",
                new { isActive, hospitalId }) > 0;
        }
    }
}
