using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class WardRepository : IWardRepository
    {
        private readonly IDbConnection _db;

        public WardRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<WardVM>> GetWardAsync()
        {
            return await _db.QueryAsync<WardVM>("SELECT * FROM tbl_Wards");
        }

        public async Task<WardVM> GetWardByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<WardVM>(
                "SELECT * FROM tbl_Wards WHERE WardId = @id", new { id });
        }
      
        public async Task<List<Ward>> GetWardNamesAsync()
        {
            var data = await _db.QueryAsync<Ward>(
                "SELECT DISTINCT WardId as Id, WardName as Name FROM tbl_Wards WHERE IsActive = 1 ORDER BY WardName"
            );

            return data.ToList();
        }
        public async Task SaveAsync(WardVM model)
        {
            if (model.WardId == 0)
            {
                await _db.ExecuteAsync(@"
                INSERT INTO tbl_Wards (WardName, IsActive)
                VALUES (@WardName, @IsActive)", model);
            }
            else
            {
                await _db.ExecuteAsync(@"
                UPDATE tbl_Wards SET
                    WardName = @WardName,
                    IsActive = @IsActive
                WHERE WardId = @WardId", model);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _db.ExecuteAsync(
                "DELETE FROM tbl_Wards WHERE WardId = @id",
                new { id });

            return result > 0;
        }
    }
}
