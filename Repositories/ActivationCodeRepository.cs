using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Repositories
{
    public class ActivationCodeRepository : IActivationCodeRepository
    {
        private readonly string _connStr;

        public ActivationCodeRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        // Only an active login counts. A disabled (for example old test) login must not stop
        // the real hospital / vendor from registering with its activation code.
        private const string HospitalHasLogin =
            "EXISTS (SELECT 1 FROM Tbl_Users u WHERE u.HospitalId = h.HospitalId AND u.RoleId = 2 AND u.IsActive = 1)";
        private const string ProviderHasLogin =
            "EXISTS (SELECT 1 FROM Tbl_Users u WHERE u.ProviderId = p.ProviderId AND u.RoleId = 3 AND u.IsActive = 1)";

        // ──────────────────────────────────────────────────────
        // Admin: who has registered, who has a code, who has neither
        // ──────────────────────────────────────────────────────
        public async Task<List<ActivationRowVM>> GetStatusAsync(string entityType)
        {
            string sql = entityType == "Hospital"
                ? $@"
                SELECT 'Hospital' AS EntityType, h.HospitalId AS EntityId, h.HospitalName AS Name,
                       d.DistrictName AS District,
                       CASE WHEN {HospitalHasLogin} THEN 'Registered'
                            WHEN c.CreatedOn IS NOT NULL THEN 'CodeIssued'
                            ELSE 'NoCode' END AS Status,
                       c.CreatedOn AS CodeIssuedOn,
                       used.UsedOn AS RegisteredOn
                FROM tbl_Hospitals h
                LEFT JOIN DistrictMaster d ON d.DistrictID = h.DistrictId
                OUTER APPLY (SELECT TOP 1 CreatedOn FROM ActivationCodes
                             WHERE EntityType = 'Hospital' AND EntityId = h.HospitalId
                               AND IsActive = 1 AND UsedOn IS NULL ORDER BY Id DESC) c
                OUTER APPLY (SELECT TOP 1 UsedOn FROM ActivationCodes
                             WHERE EntityType = 'Hospital' AND EntityId = h.HospitalId
                               AND UsedOn IS NOT NULL ORDER BY UsedOn DESC) used
                WHERE h.IsActive = 1 AND h.ApprovalStatus = 'Approved'
                ORDER BY d.DistrictName, h.HospitalName"
                : $@"
                SELECT 'Provider' AS EntityType, p.ProviderId AS EntityId,
                       COALESCE(NULLIF(p.FirmName, ''), p.ProviderName) AS Name,
                       CAST(NULL AS NVARCHAR(100)) AS District,
                       CASE WHEN {ProviderHasLogin} THEN 'Registered'
                            WHEN c.CreatedOn IS NOT NULL THEN 'CodeIssued'
                            ELSE 'NoCode' END AS Status,
                       c.CreatedOn AS CodeIssuedOn,
                       used.UsedOn AS RegisteredOn
                FROM tbl_Providers p
                OUTER APPLY (SELECT TOP 1 CreatedOn FROM ActivationCodes
                             WHERE EntityType = 'Provider' AND EntityId = p.ProviderId
                               AND IsActive = 1 AND UsedOn IS NULL ORDER BY Id DESC) c
                OUTER APPLY (SELECT TOP 1 UsedOn FROM ActivationCodes
                             WHERE EntityType = 'Provider' AND EntityId = p.ProviderId
                               AND UsedOn IS NOT NULL ORDER BY UsedOn DESC) used
                WHERE p.IsActive = 1 AND p.ApprovalStatus = 'Approved'
                ORDER BY Name";

            using var con = new SqlConnection(_connStr);
            return (await con.QueryAsync<ActivationRowVM>(sql)).ToList();
        }

        // ──────────────────────────────────────────────────────
        // Admin: issue (or re-issue) codes
        // ──────────────────────────────────────────────────────
        public async Task<List<IssuedCodeVM>> IssueAsync(
            string entityType, IReadOnlyCollection<int> entityIds,
            Func<string> newCode, int adminUserId)
        {
            var issued = new List<IssuedCodeVM>();
            if (entityIds.Count == 0) return issued;

            using var con = new SqlConnection(_connStr);
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            // Only entities that are active, approved and have no login yet
            string eligibleSql = entityType == "Hospital"
                ? $@"SELECT h.HospitalId AS EntityId, h.HospitalName AS Name, d.DistrictName AS District
                     FROM tbl_Hospitals h LEFT JOIN DistrictMaster d ON d.DistrictID = h.DistrictId
                     WHERE h.HospitalId IN @ids AND h.IsActive = 1 AND h.ApprovalStatus = 'Approved'
                       AND NOT {HospitalHasLogin}
                     ORDER BY d.DistrictName, h.HospitalName"
                : $@"SELECT p.ProviderId AS EntityId, COALESCE(NULLIF(p.FirmName, ''), p.ProviderName) AS Name,
                            CAST(NULL AS NVARCHAR(100)) AS District
                     FROM tbl_Providers p
                     WHERE p.ProviderId IN @ids AND p.IsActive = 1 AND p.ApprovalStatus = 'Approved'
                       AND NOT {ProviderHasLogin}
                     ORDER BY Name";

            var eligible = (await con.QueryAsync<IssuedCodeVM>(eligibleSql, new { ids = entityIds }, tran)).ToList();

            foreach (var e in eligible)
            {
                // The previous unused code stops working
                await con.ExecuteAsync(@"
                    UPDATE ActivationCodes SET IsActive = 0
                    WHERE EntityType = @entityType AND EntityId = @id AND IsActive = 1 AND UsedOn IS NULL",
                    new { entityType, id = e.EntityId }, tran);

                var code = newCode();
                await con.ExecuteAsync(@"
                    INSERT INTO ActivationCodes (EntityType, EntityId, CodeHash, CreatedBy)
                    VALUES (@entityType, @id, @hash, @adminUserId)",
                    new { entityType, id = e.EntityId, hash = Helpers.ActivationCodeHelper.Hash(code), adminUserId }, tran);

                e.Code = code;
                issued.Add(e);
            }

            tran.Commit();
            return issued;
        }

        // ──────────────────────────────────────────────────────
        // Register page dropdowns (only entities without a login)
        // ──────────────────────────────────────────────────────
        public async Task<List<NameOption>> GetUnclaimedHospitalsAsync(int districtId)
        {
            string sql = $@"
                SELECT h.HospitalId AS Id, h.HospitalName AS Name
                FROM tbl_Hospitals h
                WHERE h.DistrictId = @districtId AND h.IsActive = 1 AND h.ApprovalStatus = 'Approved'
                  AND NOT {HospitalHasLogin}
                ORDER BY h.HospitalName";

            using var con = new SqlConnection(_connStr);
            return (await con.QueryAsync<NameOption>(sql, new { districtId })).ToList();
        }

        public async Task<List<NameOption>> GetUnclaimedProvidersAsync()
        {
            string sql = $@"
                SELECT p.ProviderId AS Id, COALESCE(NULLIF(p.FirmName, ''), p.ProviderName) AS Name
                FROM tbl_Providers p
                WHERE p.IsActive = 1 AND p.ApprovalStatus = 'Approved'
                  AND NOT {ProviderHasLogin}
                ORDER BY Name";

            using var con = new SqlConnection(_connStr);
            return (await con.QueryAsync<NameOption>(sql)).ToList();
        }

        // ──────────────────────────────────────────────────────
        // Register with a code: attach a login to the listed hospital / vendor
        // ──────────────────────────────────────────────────────
        public async Task<ClaimResult> ClaimAsync(
            string entityType, int entityId, string codeHash,
            string phone, string email, string passwordHash)
        {
            using var con = new SqlConnection(_connStr);
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                bool isHospital = entityType == "Hospital";

                // The entity must exist, be approved and have no login yet
                string entitySql = isHospital
                    ? $@"SELECT h.HospitalName AS Name FROM tbl_Hospitals h
                         WHERE h.HospitalId = @entityId AND h.IsActive = 1 AND h.ApprovalStatus = 'Approved'
                           AND NOT {HospitalHasLogin}"
                    : $@"SELECT COALESCE(NULLIF(p.FirmName, ''), p.ProviderName) AS Name FROM tbl_Providers p
                         WHERE p.ProviderId = @entityId AND p.IsActive = 1 AND p.ApprovalStatus = 'Approved'
                           AND NOT {ProviderHasLogin}";

                var name = await con.ExecuteScalarAsync<string?>(entitySql, new { entityId }, tran);
                if (name == null)
                    return Fail(tran, "An account already exists for this hospital / firm. Please sign in.");

                // Lock the code row so two people cannot use it at once
                var codeId = await con.ExecuteScalarAsync<int?>(@"
                    SELECT Id FROM ActivationCodes WITH (UPDLOCK, ROWLOCK)
                    WHERE EntityType = @entityType AND EntityId = @entityId AND CodeHash = @codeHash
                      AND IsActive = 1 AND UsedOn IS NULL",
                    new { entityType, entityId, codeHash }, tran);

                if (codeId == null)
                {
                    tran.Rollback();
                    return new ClaimResult
                    {
                        Success = false,
                        BadCode = true,
                        Message = "The activation code is wrong, has already been used, or has been replaced. " +
                                  "Ask DGMH / Admin for a new code."
                    };
                }

                var emailTaken = await con.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM Tbl_Users WHERE Email = @email", new { email }, tran);
                if (emailTaken > 0)
                    return Fail(tran, "An account already exists for this email. Please sign in.");

                var user = new User
                {
                    FullName = name,
                    Username = email,
                    RoleId = isHospital ? 2 : 3,
                    RoleName = isHospital ? "Hospital" : "Provider",
                    HospitalId = isHospital ? entityId : 0,
                    ProviderId = isHospital ? null : entityId,
                    IsActive = true
                };

                user.UserId = await con.ExecuteScalarAsync<int>(@"
                    INSERT INTO Tbl_Users
                        (ProviderId, FullName, Email, PasswordHash, RoleId, HospitalId, IsActive, Username)
                    VALUES
                        (@ProviderId, @FullName, @Email, @PasswordHash, @RoleId, @HospitalId, 1, @Username);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new
                    {
                        user.ProviderId, user.FullName, Email = email, PasswordHash = passwordHash,
                        user.RoleId, user.HospitalId, user.Username
                    }, tran);

                // Contact details from the registration go on the hospital / vendor record
                if (isHospital)
                    await con.ExecuteAsync(
                        "UPDATE tbl_Hospitals SET Phone = @phone, Email = @email WHERE HospitalId = @entityId",
                        new { phone, email, entityId }, tran);
                else
                    await con.ExecuteAsync(
                        "UPDATE tbl_Providers SET Phone = @phone WHERE ProviderId = @entityId",
                        new { phone, entityId }, tran);

                await con.ExecuteAsync(
                    "UPDATE ActivationCodes SET UsedOn = GETDATE(), UsedByUserId = @userId WHERE Id = @codeId",
                    new { userId = user.UserId, codeId }, tran);

                tran.Commit();
                return new ClaimResult { Success = true, User = user };
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                tran.Rollback();
                return new ClaimResult { Success = false, Message = "An account already exists for this email. Please sign in." };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static ClaimResult Fail(SqlTransaction tran, string message)
        {
            tran.Rollback();
            return new ClaimResult { Success = false, Message = message };
        }
    }
}
