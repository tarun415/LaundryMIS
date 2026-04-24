

using Dapper;
using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class ProviderRepository
    {
        private readonly IDbConnection _db;

        public ProviderRepository(IDbConnection db)
        {
            _db = db;
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
        public async Task UpdateAsync(ProvidersVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            await _db.ExecuteAsync(@"
        UPDATE tbl_Providers
        SET 
            ProviderName = @ProviderName,
            RatePerBed = @RatePerBed,
            FirmName = @FirmName,
            IsActive= @IsActive
        WHERE ProviderId = @ProviderId
    ", model);
        }
        public async Task<IEnumerable<ProvidersVM>> GetProviderAsync()
        {
            return await _db.QueryAsync<ProvidersVM>(
                "SELECT ProviderId,ProviderName,IsActive,FirmName,RatePerBed FROM tbl_Providers WHERE IsActive = 1"
            );
        }
        public async Task<ProvidersVM> GetProviderByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<ProvidersVM>(@"
        SELECT ProviderId, ProviderName, FirmName, RatePerBed, IsActive
        FROM tbl_Providers 
        WHERE ProviderId = @Id
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