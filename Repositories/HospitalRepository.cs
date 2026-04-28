using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly IDbConnection _db;

        public HospitalRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HospitalVM>> GetAllAsync()
        {
            return await _db.QueryAsync<HospitalVM>("SELECT * FROM Tbl_Hospitals WHERE IsActive = 1");
        }

        public async Task InsertAsync(HospitalVM model)
        {
            var sql = @"INSERT INTO Tbl_Hospitals
                        (HospitalName, DistrictId, Address, City, ContactPerson, Phone, Email, IsActive)
                        VALUES
                        (@HospitalName, @DistrictId, @Address, @City, @ContactPerson, @Phone, @Email, 1)";

            await _db.ExecuteAsync(sql, model);
        }

        public async Task UpdateAsync(HospitalVM model)
        {
            var sql = @"UPDATE Tbl_Hospitals
                        SET HospitalName=@HospitalName,
                            DistrictId=@DistrictId,
                            Address=@Address,
                            City=@City,
                            ContactPerson=@ContactPerson,
                            Phone=@Phone,
                            Email=@Email
                        WHERE HospitalId=@HospitalId";

            await _db.ExecuteAsync(sql, model);
        }

        public async Task<HospitalVM?> GetHospitalByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<HospitalVM>(
                "SELECT * FROM Tbl_Hospitals WHERE HospitalId=@id",
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
                "SELECT DistrictId, DistrictName FROM DistrictMaster");

            return data.ToList();
        }

        public async Task DeleteAsync(int id)
        {
            await _db.ExecuteAsync(
                "UPDATE Tbl_Hospitals SET IsActive = 0 WHERE HospitalId=@id",
                new { id });
        }
    }
}