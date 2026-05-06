

using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace LaudaryMis.Repositories
{
    public class ProviderRepository: IProviderRepository
    {
        private readonly IDbConnection _db;
        private readonly IConfiguration _config;

        public ProviderRepository(IDbConnection db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            return await _db.QueryAsync<Provider>(
                "SELECT * FROM tbl_Providers WHERE IsActive = 1"
            );
        }
     
        
        public async Task InsertAsync(ProvidersVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            await _db.ExecuteAsync(@"
            INSERT INTO tbl_Providers
            (ProviderName, RatePerBed, FirmName, IsActive,CreatedDBY)
            VALUES
            (@ProviderName, @RatePerBed, @FirmName,@IsActive,'Admin')
        ", model);
        }
        public async Task SaveProviderWithLogin(ProvidersVM model)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await con.OpenAsync();

            using var tran = con.BeginTransaction();

            try
            {
                //  Insert Provider
                var providerId = await con.ExecuteScalarAsync<int>(@"
            INSERT INTO tbl_Providers
            (ProviderName, RatePerBed, FirmName, IsActive,CreatedDBY)
            VALUES
            (@ProviderName, @RatePerBed, @FirmName,@IsActive,'Admin');

            SELECT CAST(SCOPE_IDENTITY() as int);
        ", model, tran);

                // 2️⃣ Generate Username (safe)
                var username = "PR_" + model.ProviderName?.Split('@')[0]?.ToLower() ?? "hospital";

                // 3️⃣ Generate Password
                var rawPassword = model.Password; // default
                var hashedPassword = rawPassword;
                // var hashedPassword = HashPassword(rawPassword);

                // 4️⃣ Insert User
                await con.ExecuteAsync(@"
            INSERT INTO Tbl_Users
            (ProviderId, FullName, Email, PasswordHash, RoleId, HospitalId, IsActive, Username)
            VALUES
            (@providerId, @FullName, @Email, @PasswordHash, 3, 0, 1, @Username)
        ", new
                {
                    FullName = model.ProviderName,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    providerId = providerId,
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
    //    public async Task UpdateAsync(ProvidersVM model)
    //    {
    //        if (_db.State == ConnectionState.Closed)
    //            _db.Open();

    //        await _db.ExecuteAsync(@"
    //    UPDATE tbl_Providers
    //    SET 
    //        ProviderName = @ProviderName,
    //        RatePerBed = @RatePerBed,
    //        FirmName = @FirmName,
    //        IsActive= @IsActive
    //    WHERE ProviderId = @ProviderId
    //", model);
    //    }
        public async Task UpdateAsync(ProvidersVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    // 1️⃣ Update Provider
                    await _db.ExecuteAsync(@"
                UPDATE tbl_Providers
                SET 
                    ProviderName = @ProviderName,
                    RatePerBed = @RatePerBed,
                    FirmName = @FirmName,
                    IsActive = @IsActive
                WHERE ProviderId = @ProviderId
            ", model, tran);

                    // 2️⃣ Update User (with optional password)
                    await _db.ExecuteAsync(@"
                UPDATE Tbl_Users
                SET 
                    FullName = @ProviderName,
                    Email = @Email,
                    PasswordHash = CASE 
                        WHEN @PasswordHash IS NULL OR @PasswordHash = '' 
                        THEN PasswordHash 
                        ELSE @PasswordHash 
                    END
                WHERE ProviderId = @ProviderId
                  AND RoleId = 3
            ", new
                    {
                        model.ProviderName,
                        model.Email,
                        model.ProviderId,
                        PasswordHash = model.Password // send null if not changing
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

        public async Task SaveAsync(ProvidersVM model)
        {
            if (model.ProviderId == 0)
                await InsertAsync(model);
            else
                await UpdateAsync(model);
        }

        public async Task<IEnumerable<ProvidersVM>> GetProviderAsync()
        {
            return await _db.QueryAsync<ProvidersVM>(
                "SELECT pr.ProviderId ,pr.ProviderName,pr.FirmName,pr.NoOfBeds,pr.RatePerBed,pr.IsActive,dm.DistrictID, dm.DistrictName,us.PasswordHash as [Password],us.Email  FROM tbl_Providers as pr left join DistrictMaster as dm on pr.ProviderId=dm.DistrictID left join Tbl_Users us on us.ProviderId=pr.ProviderId  WHERE pr.IsActive = 1"
            );
        }

        public async Task CreateProviderWithLogin(ProvidersVM model)
        {
            if ( model.ProviderId == 0)
                await SaveProviderWithLogin(model);
            else
                await UpdateAsync(model);
        }
        public async Task<ProvidersVM> GetProviderByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<ProvidersVM>(@"
        SELECT pr.ProviderId ,pr.ProviderName,pr.FirmName,pr.NoOfBeds,pr.RatePerBed,pr.IsActive,dm.DistrictID, dm.DistrictName,us.PasswordHash as [Password],us.Email  FROM tbl_Providers as pr left join DistrictMaster as dm on pr.ProviderId=dm.DistrictID left join Tbl_Users us on us.ProviderId=pr.ProviderId 
        WHERE pr.ProviderId = @Id
    ", new { Id = id });
        }
        public async Task DeleteAsync(int id)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            await _db.ExecuteAsync(@"
        UPDATE tbl_Providers
        SET IsActive = 0
        WHERE ProviderId = @Id
    ", new { Id = id });
        }
       
    }

}