using Dapper;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly string _connStr;

        public BillRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private SqlConnection Conn() => new(_connStr);

        // WeeklyPerformanceReport.Month is an nvarchar that holds the month number ("5")
        public async Task<List<BillMonthVM>> GetMonthRowsAsync(
            int providerId, int? hospitalId = null, int? month = null, int? year = null)
        {
            const string sql = @"
                SELECT w.HospitalId, h.HospitalName, w.Year,
                       TRY_CAST(w.Month AS INT) AS Month,
                       COUNT(DISTINCT CASE WHEN w.Status = 'Verified' THEN w.Week END) AS VerifiedWeeks,
                       COUNT(DISTINCT CASE WHEN w.Status = 'Pending'  THEN w.Week END) AS PendingWeeks,
                       AVG(CASE WHEN w.Status = 'Verified' THEN CAST(w.TotalScore AS DECIMAL(6,2)) END) AS AvgScore
                FROM WeeklyPerformanceReport w
                JOIN tbl_Hospitals h ON h.HospitalId = w.HospitalId
                WHERE w.ProviderId = @providerId
                  AND TRY_CAST(w.Month AS INT) BETWEEN 1 AND 12
                  AND (@hospitalId IS NULL OR w.HospitalId = @hospitalId)
                  AND (@month      IS NULL OR TRY_CAST(w.Month AS INT) = @month)
                  AND (@year       IS NULL OR w.Year = @year)
                GROUP BY w.HospitalId, h.HospitalName, w.Year, TRY_CAST(w.Month AS INT)
                ORDER BY w.Year DESC, TRY_CAST(w.Month AS INT) DESC, h.HospitalName";

            using var con = Conn();
            return (await con.QueryAsync<BillMonthVM>(sql, new { providerId, hospitalId, month, year })).ToList();
        }

        public async Task<BillAgreementInfo?> GetAgreementInfoAsync(int providerId, int hospitalId)
        {
            const string sql = @"
                SELECT a.Id AS AgreementId, a.HospitalId, a.ProviderId,
                       a.BedCount AS SanctionedBeds, a.RatePerBed AS RatePerBedPerYear,
                       h.HospitalName, dm.DistrictName AS District,
                       a.AgreementFile AS ContractNo, p.ProviderName
                FROM ProviderHospitalAgreements a
                JOIN tbl_Hospitals h ON a.HospitalId = h.HospitalId
                LEFT JOIN DistrictMaster dm ON h.DistrictId = dm.DistrictID
                JOIN tbl_Providers p ON a.ProviderId = p.ProviderId
                WHERE a.ProviderId = @providerId AND a.HospitalId = @hospitalId AND a.IsActive = 1";

            using var con = Conn();
            return await con.QueryFirstOrDefaultAsync<BillAgreementInfo>(sql, new { providerId, hospitalId });
        }

        public async Task<List<BillPrintLogVM>> GetPrintLogAsync(int providerId)
        {
            const string sql = @"
                SELECT l.HospitalId, h.HospitalName, l.BillMonth, l.BillYear, l.Version,
                       l.WPRAvgScore, l.NetPayableAmount, l.PrintedAt
                FROM BillPrintLog l
                JOIN tbl_Hospitals h ON h.HospitalId = l.HospitalId
                WHERE l.ProviderId = @providerId
                ORDER BY l.PrintedAt DESC, l.Id DESC";

            using var con = Conn();
            return (await con.QueryAsync<BillPrintLogVM>(sql, new { providerId })).ToList();
        }

        public async Task InsertPrintLogAsync(int providerId, int hospitalId, int month, int year, int version,
                                              decimal avgScore, decimal netPayable, int userId)
        {
            const string sql = @"
                INSERT INTO BillPrintLog
                    (ProviderId, HospitalId, BillMonth, BillYear, Version, WPRAvgScore, NetPayableAmount, PrintedBy)
                VALUES
                    (@providerId, @hospitalId, @month, @year, @version, @avgScore, @netPayable, @userId)";

            using var con = Conn();
            await con.ExecuteAsync(sql, new { providerId, hospitalId, month, year, version, avgScore, netPayable, userId });
        }

        public async Task<List<DisputeCountVM>> GetDisputeCountsAsync(int providerId)
        {
            const string sql = @"
                SELECT HospitalId, BillMonth, BillYear,
                       SUM(CASE WHEN Status = 'Resolved' THEN 1 ELSE 0 END) AS Resolved,
                       SUM(CASE WHEN Status = 'Open'     THEN 1 ELSE 0 END) AS Opened
                FROM BillDisputes
                WHERE ProviderId = @providerId
                GROUP BY HospitalId, BillMonth, BillYear";

            using var con = Conn();
            return (await con.QueryAsync<DisputeCountVM>(sql, new { providerId })).ToList();
        }
    }
}
