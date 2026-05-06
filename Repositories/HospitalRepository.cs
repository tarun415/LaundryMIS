using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly IDbConnection _db;
        private readonly IConfiguration _config;

        public HospitalRepository(IDbConnection db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<IEnumerable<HospitalVM>> GetAllAsync()
        {
            return await _db.QueryAsync<HospitalVM>("SELECT hs.HospitalId ,hs.HospitalName,hs.Address,hs.ContactPerson,hs.Phone,hs.Email,hs.IsActive,dm.DistrictID, dm.DistrictName FROM Tbl_Hospitals as hs left join DistrictMaster as dm on hs.DistrictId=dm.DistrictID WHERE hs.IsActive = 1");
        }

        public async Task InsertAsync(HospitalVM model)
        {
            var sql = @"INSERT INTO Tbl_Hospitals
                        (HospitalName, DistrictId, Address, City, ContactPerson, Phone, Email, IsActive)
                        VALUES
                        (@HospitalName, @DistrictId, @Address, @City, @ContactPerson, @Phone, @Email, 1)";

            await _db.ExecuteAsync(sql, model);
        }
        public async Task CreateHospitalWithLogin(HospitalVM model)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();

            using var tran = con.BeginTransaction();

            try
            {
                // 1️⃣ Insert Hospital
                var hospitalId = await con.ExecuteScalarAsync<int>(@"
            INSERT INTO Tbl_Hospitals
            (HospitalName, DistrictId, Address, ContactPerson, Phone, Email, IsActive)
            VALUES
            (@HospitalName, @DistrictId, @Address, @ContactPerson, @Phone, @Email, 1);

            SELECT CAST(SCOPE_IDENTITY() as int);
        ", model, tran);

                // 2️⃣ Generate Username (safe)
                var username = "HO_" + model.HospitalName?.Split('@')[0]?.ToLower() ?? "hospital";

                // 3️⃣ Generate Password
                var rawPassword = model.Password; // default
                var hashedPassword = rawPassword; 
                // var hashedPassword = HashPassword(rawPassword);

                // 4️⃣ Insert User
                await con.ExecuteAsync(@"
            INSERT INTO Tbl_Users
            (ProviderId, FullName, Email, PasswordHash, RoleId, HospitalId, IsActive, Username)
            VALUES
            (Null, @FullName, @Email, @PasswordHash, 2, @HospitalId, 1, @Username)
        ", new
                {
                    FullName = model.ContactPerson,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    HospitalId = hospitalId,
                    Username = username
                }, tran);

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
        //public async Task UpdateAsync(HospitalVM model)
        //{
        //    var sql = @"UPDATE Tbl_Hospitals
        //                SET HospitalName=@HospitalName,
        //                    DistrictId=@DistrictId,
        //                    Address=@Address,

        //                    ContactPerson=@ContactPerson,
        //                    Phone=@Phone,
        //                    Email=@Email
        //                WHERE HospitalId=@HospitalId";

        //    await _db.ExecuteAsync(sql, model);
        //}
        public async Task UpdateAsync(HospitalVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    // 1️⃣ Update Hospital
                    await _db.ExecuteAsync(@"
                UPDATE Tbl_Hospitals
                SET 
                    HospitalName = @HospitalName,
                    DistrictId = @DistrictId,
                    Address = @Address,
                    ContactPerson = @ContactPerson,
                    Phone = @Phone,
                    Email = @Email
                WHERE HospitalId = @HospitalId
            ", model, tran);

                    // 2️ Update User linked with Hospital
                    await _db.ExecuteAsync(@"
                UPDATE Tbl_Users
                SET 
                    FullName = @HospitalName,
                    Email = @Email,
                    PasswordHash = CASE 
                        WHEN @PasswordHash IS NULL OR @PasswordHash = '' 
                        THEN PasswordHash 
                        ELSE @PasswordHash 
                    END
                WHERE HospitalId = @HospitalId
                  AND RoleId = 2
            ", new
                    {
                        model.HospitalName,
                        model.Email,
                        model.HospitalId,
                        PasswordHash = model.Password // null if not changing
                    }, tran);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public async Task<HospitalVM?> GetHospitalByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<HospitalVM>(
                "SELECT hs.HospitalId ,hs.HospitalName,hs.Address,hs.ContactPerson,hs.Phone,hs.Email,hs.IsActive,dm.DistrictID, dm.DistrictName,us.PasswordHash as [Password] FROM Tbl_Hospitals as hs left join DistrictMaster as dm on hs.DistrictId=dm.DistrictID left join Tbl_Users us on us.HospitalId=hs.HospitalId  WHERE hs.HospitalId=@id",
                new { id });
        }

        public async Task<List<Hospital>> GetHospitalsByDistrict(int districtId)
        {
            var data = await _db.QueryAsync<Hospital>(
                "SELECT * FROM Tbl_Hospitals WHERE DistrictId=@districtId",
                new { districtId });

            return data.ToList();
        }

        public async Task<List<District>> GetDistricts()
        {
            var data = await _db.QueryAsync<District>(
                "SELECT DistrictId, DistrictName FROM DistrictMaster where StateID=34");

            return data.ToList();
        }

        public async Task DeleteAsync(int id)
        {
            await _db.ExecuteAsync(
                "UPDATE Tbl_Hospitals SET IsActive = 0 WHERE HospitalId=@id",
                new { id });
        }
        public async Task<List<GetHospital>> GetHospitalNamesAsync()
        {
            var data = await _db.QueryAsync<GetHospital>(
                "SELECT DISTINCT HospitalId as Id, HospitalName as Name FROM Tbl_Hospitals WHERE IsActive = 1 ORDER BY HospitalName"
            );

            return data.ToList();
        }
    }
}