using Dapper;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Repositories
{
    public class DisputeRepository : IDisputeRepository
    {
        private readonly string _connStr;

        public DisputeRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private SqlConnection Conn() => new(_connStr);

        private const string SelectList = @"
            SELECT d.Id, d.ProviderId, p.ProviderName, d.HospitalId, h.HospitalName, dm.DistrictName,
                   d.BillMonth, d.BillYear, d.Remarks, d.Status, d.RaisedAt, d.ResolvedAt,
                   d.ResolutionRemarks, d.LetterFile, d.LetterOriginalName
            FROM BillDisputes d
            JOIN tbl_Hospitals h ON h.HospitalId = d.HospitalId
            LEFT JOIN DistrictMaster dm ON dm.DistrictID = h.DistrictId
            JOIN tbl_Providers p ON p.ProviderId = d.ProviderId";

        public async Task<bool> InsertAsync(int providerId, int hospitalId, int month, int year, string remarks, int userId)
        {
            using var con = Conn();
            try
            {
                await con.ExecuteAsync(@"
                    INSERT INTO BillDisputes (ProviderId, HospitalId, BillMonth, BillYear, Remarks, RaisedBy)
                    VALUES (@providerId, @hospitalId, @month, @year, @remarks, @userId)",
                    new { providerId, hospitalId, month, year, remarks, userId });
                return true;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                // Unique index UX_BillDisputes_OneOpen
                return false;
            }
        }

        public async Task<List<DisputeListItemVM>> GetListAsync(int? providerId, int? hospitalId, string? status)
        {
            var sql = SelectList + @"
                WHERE (@providerId IS NULL OR d.ProviderId = @providerId)
                  AND (@hospitalId IS NULL OR d.HospitalId = @hospitalId)
                  AND (@status     IS NULL OR d.Status     = @status)
                ORDER BY CASE WHEN d.Status = 'Open' THEN 0 ELSE 1 END, d.RaisedAt DESC";

            using var con = Conn();
            return (await con.QueryAsync<DisputeListItemVM>(sql, new { providerId, hospitalId, status })).ToList();
        }

        public async Task<DisputeListItemVM?> GetByIdAsync(int id)
        {
            using var con = Conn();
            return await con.QueryFirstOrDefaultAsync<DisputeListItemVM>(SelectList + " WHERE d.Id = @id", new { id });
        }

        public async Task<int> CountOpenAsync(int? hospitalId)
        {
            using var con = Conn();
            return await con.ExecuteScalarAsync<int>(@"
                SELECT COUNT(*) FROM BillDisputes
                WHERE Status = 'Open' AND (@hospitalId IS NULL OR HospitalId = @hospitalId)",
                new { hospitalId });
        }

        public async Task<bool> ResolveAsync(int id, int adminId, string remarks, string? letterFile, string? letterOriginalName)
        {
            using var con = Conn();
            return await con.ExecuteAsync(@"
                UPDATE BillDisputes
                SET Status = 'Resolved', ResolvedBy = @adminId, ResolvedAt = GETDATE(),
                    ResolutionRemarks = @remarks, LetterFile = @letterFile, LetterOriginalName = @letterOriginalName
                WHERE Id = @id AND Status = 'Open'",
                new { id, adminId, remarks, letterFile, letterOriginalName }) > 0;
        }

        public async Task<bool> RejectAsync(int id, int adminId, string remarks)
        {
            using var con = Conn();
            return await con.ExecuteAsync(@"
                UPDATE BillDisputes
                SET Status = 'Rejected', ResolvedBy = @adminId, ResolvedAt = GETDATE(), ResolutionRemarks = @remarks
                WHERE Id = @id AND Status = 'Open'",
                new { id, adminId, remarks }) > 0;
        }
    }
}
