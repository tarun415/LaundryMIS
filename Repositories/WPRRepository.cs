using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class WPRRepository : IWPRRepository
    {
        private readonly string _connStr;

        public WPRRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private IDbConnection CreateConnection() =>
            new SqlConnection(_connStr);

        // AGREEMENTS
        public async Task<IEnumerable<AgreementVM>> GetHospitalAgreements(int hospitalId)
        {
            const string sql = @"
                SELECT a.Id, h.HospitalName
                FROM ProviderHospitalAgreements a
                JOIN tbl_Hospitals h ON a.HospitalId = h.HospitalId
                WHERE a.HospitalId = @hospitalId AND a.IsActive = 1";

            using var conn = CreateConnection();
            return await conn.QueryAsync<AgreementVM>(sql, new { hospitalId });
        }

        // ✅ Duplicate check
        public async Task<bool> WPRExistsAsync(int week, string month, int year, string staffName)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM WeeklyPerformanceReport
                WHERE Week      = @week
                  AND Month     = @month
                  AND Year      = @year
                  AND StaffName = @staffName";

            using var conn = CreateConnection();
            int count = await conn.QuerySingleAsync<int>(sql, new { week, month, year, staffName });
            return count > 0;
        }

        public async Task<int> InsertWPRAsync(WeeklyPerformanceReport wpr)
        {
            const string sql = @"
                INSERT INTO WeeklyPerformanceReport
                    (Week, Month, Year, StaffName, Remarks,
                     TotalScore, PaymentPercentage, SubmittedAt, AgreementId, ProviderId, HospitalId)
                VALUES
                    (@Week, @Month, @Year, @StaffName, @Remarks,
                     @TotalScore, @PaymentPercentage, @SubmittedAt, @AgreementId, @ProviderId, @HospitalId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = CreateConnection();
            return await conn.QuerySingleAsync<int>(sql, wpr);
        }

        public async Task InsertWPRDetailsAsync(IEnumerable<WPRDetail> details)
        {
            const string sql = @"
                INSERT INTO WPRDetail
                    (WPRId, ParameterId, ParameterName, Score)
                VALUES
                    (@WPRId, @ParameterId, @ParameterName, @Score);";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, details);
        }

        public async Task<bool> CheckWeeklyVerification(int weekNo, int month, int year)
        {
            DateTime fromDate;
            DateTime toDate;

            switch (weekNo)
            {
                case 1:
                    fromDate = new DateTime(year, month, 1);
                    toDate = new DateTime(year, month, 7);
                    break;

                case 2:
                    fromDate = new DateTime(year, month, 8);
                    toDate = new DateTime(year, month, 14);
                    break;

                case 3:
                    fromDate = new DateTime(year, month, 15);
                    toDate = new DateTime(year, month, 21);
                    break;

                case 4:
                    fromDate = new DateTime(year, month, 22);
                    toDate = new DateTime(year, month, 28);
                    break;

                case 5:
                    fromDate = new DateTime(year, month, 29);

                    toDate = new DateTime(
                        year,
                        month,
                        DateTime.DaysInMonth(year, month));

                    break;

                default:
                    return false;
            }

            const string sql = @"

SELECT COUNT(1)
FROM WeeklyVerificationLog
WHERE Status = 'Verified'

AND CAST(FromDate AS DATE)
    BETWEEN CAST(@fromDate AS DATE)
        AND CAST(@toDate AS DATE)

AND CAST(ToDate AS DATE)
    BETWEEN CAST(@fromDate AS DATE)
        AND CAST(@toDate AS DATE)";

            using var conn = CreateConnection();

            int count = await conn.QuerySingleAsync<int>(
                sql,
                new
                {
                    fromDate,
                    toDate
                });

            return count > 0;
        }
    }
}
