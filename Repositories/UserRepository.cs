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
    
        // ── Hospital / vendor sign-in with a mobile number or an email ──────────

        private const string ContactLoginFailed = "Incorrect mobile number / email or password.";

        private const string ContactLoginAmbiguous =
            "More than one account uses this mobile number and password. Please sign in with your email instead.";

        // A bcrypt hash that no password matches: an unknown account takes as long to refuse as a wrong password
        private static readonly string UnknownAccountHash = PasswordHasher.Hash("no-account-has-this-password");

        // Of the accounts found for the mobile number / email, the one this password belongs to
        // (an email is unique; a mobile number can be on more than one old record).
        private static (User? User, LoginResult? Refusal) PickByPassword(List<User> candidates, string password)
        {
            if (candidates.Count == 0)
            {
                PasswordHasher.Verify(password, UnknownAccountHash);
                return (null, new LoginResult { Success = false, Message = ContactLoginFailed });
            }

            var matches = candidates.Where(u => PasswordHasher.Verify(password, u.PasswordHash)).ToList();
            if (matches.Count == 0)
                return (null, new LoginResult { Success = false, Message = ContactLoginFailed });
            if (matches.Count > 1)
                return (null, new LoginResult { Success = false, Message = ContactLoginAmbiguous });

            return (matches[0], null);
        }

        public async Task<LoginResult> LoginHospital(string loginId, string password)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            // loginId is an email or a mobile number; the mobile number lives on the hospital record
            var sql = loginId.Contains('@')
                ? @"SELECT u.*, r.RoleName
                    FROM Tbl_Users u
                    INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                    WHERE u.RoleId = 2 AND u.IsActive = 1 AND u.Email = @LoginId"
                : @"SELECT u.*, r.RoleName
                    FROM Tbl_Users u
                    INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                    INNER JOIN Tbl_Hospitals h ON h.HospitalId = u.HospitalId
                    WHERE u.RoleId = 2 AND u.IsActive = 1 AND h.Phone = @LoginId";

            var candidates = (await con.QueryAsync<User>(sql, new { LoginId = loginId })).ToList();
            var (user, refusal) = PickByPassword(candidates, password);
            if (user == null)
                return refusal!;

            var hospitalBlock = await ApprovalBlockAsync(con,
                "SELECT ApprovalStatus, ApprovalRemarks FROM Tbl_Hospitals WHERE HospitalId = @Id",
                user.HospitalId, "hospital");
            if (hospitalBlock != null)
                return hospitalBlock;

            if (PasswordHasher.NeedsRehash(user.PasswordHash))
            {
                await con.ExecuteAsync(
                    "UPDATE Tbl_Users SET PasswordHash = @Hash WHERE UserId = @UserId",
                    new { Hash = PasswordHasher.Hash(password), user.UserId });
            }

            // Older hospital users were saved with a placeholder name such as
            // "." (the contact person), which leaves the navbar blank. Fall
            // back to the hospital's own name in that case.
            if (!(user.FullName ?? "").Any(char.IsLetterOrDigit))
            {
                var hospitalName = await con.ExecuteScalarAsync<string?>(
                    "SELECT HospitalName FROM Tbl_Hospitals WHERE HospitalId = @HospitalId",
                    new { user.HospitalId });
                if (!string.IsNullOrWhiteSpace(hospitalName))
                    user.FullName = hospitalName;
            }

            return new LoginResult
            {
                Success = true,
                User = user
            };
        }

        public async Task<LoginResult> LoginProvider(string loginId, string password)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            // loginId is an email or a mobile number; the mobile number lives on the vendor record
            var sql = loginId.Contains('@')
                ? @"SELECT u.*, r.RoleName
                    FROM Tbl_Users u
                    INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                    WHERE u.RoleId = 3 AND u.IsActive = 1 AND u.Email = @LoginId"
                : @"SELECT u.*, r.RoleName
                    FROM Tbl_Users u
                    INNER JOIN Tbl_Roles r ON u.RoleId = r.RoleId
                    INNER JOIN tbl_Providers p ON p.ProviderId = u.ProviderId
                    WHERE u.RoleId = 3 AND u.IsActive = 1 AND p.Phone = @LoginId";

            var candidates = (await con.QueryAsync<User>(sql, new { LoginId = loginId })).ToList();
            var (user, refusal) = PickByPassword(candidates, password);
            if (user == null)
                return refusal!;

            var providerBlock = await ApprovalBlockAsync(con,
                "SELECT ApprovalStatus, ApprovalRemarks FROM tbl_Providers WHERE ProviderId = @Id",
                user.ProviderId, "provider");
            if (providerBlock != null)
                return providerBlock;

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

        private class ApprovalRow
        {
            public string? ApprovalStatus { get; set; }
            public string? ApprovalRemarks { get; set; }
        }

        // Hospitals and providers can only sign in once an admin has approved them.
        private static async Task<LoginResult?> ApprovalBlockAsync(
            SqlConnection con, string sql, int? id, string what)
        {
            var row = await con.QueryFirstOrDefaultAsync<ApprovalRow>(sql, new { Id = id });

            if (row == null || row.ApprovalStatus == null || row.ApprovalStatus == "Approved")
                return null;

            var message = row.ApprovalStatus == "Rejected"
                ? $"Your {what} registration was rejected."
                  + (string.IsNullOrWhiteSpace(row.ApprovalRemarks) ? "" : $" Reason: {row.ApprovalRemarks}")
                  + " Please contact the administrator."
                : $"Your {what} registration is pending admin approval. You can sign in once it is approved.";

            return new LoginResult { Success = false, Message = message };
        }

        // ──────────────────────────────────────────────────────
        // Self-registration: hospital/provider record + login user,
        // ek hi transaction mein. Admin approval nahi chahiye — isliye
        // ApprovalStatus seedha 'Approved' (Admin se bane records 'Pending' se shuru hote hain).
        // ──────────────────────────────────────────────────────
        public async Task<LoginResult> RegisterHospital(RegisterVM model)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                var email = model.Email.Trim();

                if (await EmailExists(con, tran, email))
                    return Fail(tran, "An account already exists for this email. Please sign in.");

                var districtOk = await con.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM DistrictMaster WHERE DistrictID = @DistrictId",
                    new { model.DistrictId }, tran);

                if (districtOk == 0)
                    return Fail(tran, "Select a valid district.");

                var duplicate = await con.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(*) FROM Tbl_Hospitals
                    WHERE DistrictId = @DistrictId
                      AND LTRIM(RTRIM(HospitalName)) = @HospitalName",
                    new { model.DistrictId, HospitalName = model.HospitalName!.Trim() }, tran);

                if (duplicate > 0)
                    return Fail(tran, "A hospital with this name is already on the tender list for this district. Do not create a new account: open 'Register' from the Login page, choose your hospital and enter the activation code DGMH gave you.");

                var hospitalId = await con.ExecuteScalarAsync<int>(@"
                    INSERT INTO Tbl_Hospitals
                        (HospitalName, DistrictId, Address, ContactPerson, Phone, Email, IsActive, ApprovalStatus)
                    VALUES
                        (@HospitalName, @DistrictId, @Address, @ContactPerson, @Phone, @Email, 1, 'Approved');
                    SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new
                    {
                        HospitalName = model.HospitalName.Trim(),
                        model.DistrictId,
                        Address = model.Address?.Trim(),
                        ContactPerson = model.ContactPerson?.Trim(),
                        model.Phone,
                        Email = email
                    }, tran);

                var user = new User
                {
                    FullName = model.HospitalName.Trim(),
                    Username = email,
                    RoleId = 2,
                    RoleName = "Hospital",
                    HospitalId = hospitalId,
                    IsActive = true
                };

                user.UserId = await InsertUser(con, tran, user, email, model.Password);

                tran.Commit();
                return new LoginResult { Success = true, User = user };
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                // Unique index on Tbl_Users.Email (do log ek saath register karein)
                tran.Rollback();
                return new LoginResult { Success = false, Message = "An account already exists for this email. Please sign in." };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<LoginResult> RegisterProvider(RegisterVM model)
        {
            using var con = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();
            using var tran = con.BeginTransaction();

            try
            {
                var email = model.Email.Trim();

                if (await EmailExists(con, tran, email))
                    return Fail(tran, "An account already exists for this email. Please sign in.");

                var duplicate = await con.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(*) FROM tbl_Providers
                    WHERE LTRIM(RTRIM(FirmName)) = @FirmName",
                    new { FirmName = model.FirmName!.Trim() }, tran);

                if (duplicate > 0)
                    return Fail(tran, "A firm with this name is already on the tender list. Do not create a new account: open 'Register' from the Login page, choose your firm and enter the activation code DGMH gave you.");

                var providerId = await con.ExecuteScalarAsync<int>(@"
                    INSERT INTO tbl_Providers
                        (ProviderName, FirmName, Phone, IsActive, CreatedDBY, ApprovalStatus)
                    VALUES
                        (@ProviderName, @FirmName, @Phone, 1, 'Self-registration', 'Approved');
                    SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new
                    {
                        ProviderName = model.ProviderName!.Trim(),
                        FirmName = model.FirmName.Trim(),
                        model.Phone
                    }, tran);

                var user = new User
                {
                    FullName = model.ProviderName.Trim(),
                    Username = email,
                    RoleId = 3,
                    RoleName = "Provider",
                    ProviderId = providerId,
                    HospitalId = 0,   // Tbl_Users.HospitalId NOT NULL hai; providers ke liye 0
                    IsActive = true
                };

                user.UserId = await InsertUser(con, tran, user, email, model.Password);

                tran.Commit();
                return new LoginResult { Success = true, User = user };
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                tran.Rollback();
                return new LoginResult { Success = false, Message = "An account already exists for this email. Please sign in." };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static async Task<bool> EmailExists(
            SqlConnection con, SqlTransaction tran, string email)
        {
            return await con.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Tbl_Users WHERE Email = @Email",
                new { Email = email }, tran) > 0;
        }

        private static async Task<int> InsertUser(
            SqlConnection con, SqlTransaction tran, User user, string email, string password)
        {
            return await con.ExecuteScalarAsync<int>(@"
                INSERT INTO Tbl_Users
                    (ProviderId, FullName, Email, PasswordHash, RoleId, HospitalId, IsActive, Username)
                VALUES
                    (@ProviderId, @FullName, @Email, @PasswordHash, @RoleId, @HospitalId, 1, @Username);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new
                {
                    user.ProviderId,
                    user.FullName,
                    Email = email,
                    PasswordHash = PasswordHasher.Hash(password),
                    user.RoleId,
                    HospitalId = user.HospitalId ?? 0,
                    user.Username
                }, tran);
        }

        private static LoginResult Fail(SqlTransaction tran, string message)
        {
            tran.Rollback();
            return new LoginResult { Success = false, Message = message };
        }
    }
}