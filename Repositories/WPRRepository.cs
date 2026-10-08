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

        // ✅ Duplicate check (per hospital: another hospital's report for the same vendor must not block this one)
        public async Task<bool> WPRExistsAsync(int hospitalId, int week, string month, int year, string staffName)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM WeeklyPerformanceReport
                WHERE HospitalId = @hospitalId
                  AND Week      = @week
                  AND Month     = @month
                  AND Year      = @year
                  AND StaffName = @staffName";

            using var conn = CreateConnection();
            int count = await conn.QuerySingleAsync<int>(sql, new { hospitalId, week, month, year, staffName });
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

        public async Task<bool> CheckWeeklyVerification(int hospitalId, int weekNo, int month, int year)
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

AND HospitalId = @hospitalId

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
                    hospitalId,
                    fromDate,
                    toDate
                });

            return count > 0;
        }
        public async Task<List<WeeklyPerformanceVM>> GetWeeklyPerformanceData( int agreementId, int hospitalId, int weekNo, int month, int year)
        {
            using var conn = CreateConnection();

            var result = await conn.QueryAsync<WeeklyPerformanceVM>(
                "sp_GetWeeklyPerformanceData",
                new
                {
                    AgreementId = agreementId,
                    HospitalId = hospitalId,
                    WeekNo = weekNo,
                    Month = month,
                    Year = year
                },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<int> InsertWPREntryAsync(WPREntry entry)
        {
            const string sql = @"

INSERT INTO WPREntries
(
    AgreementId,
    HospitalId,
    WeekStart,
    WeekEnd,
    TotalScore,
    CreatedOn,
    ProviderId,
    MonthNo,
    YearNo,
    WeekNo,
    PerformanceGrade,
    Remarks
)
VALUES
(
    @AgreementId,
    @HospitalId,
    @WeekStart,
    @WeekEnd,
    @TotalScore,
    @CreatedOn,
    @ProviderId,
    @MonthNo,
    @YearNo,
    @WeekNo,
    @PerformanceGrade,
    @Remarks
);

SELECT CAST(SCOPE_IDENTITY() AS INT);

";

            using var conn = CreateConnection();

            return await conn.QuerySingleAsync<int>(sql, entry);
        }


        public async Task<int> SaveWPRAsync(
    WeeklyPerformanceReport wpr,
    WPREntry entry,
    List<WPRDetail> details)
        {
            using var conn = CreateConnection();

             conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                //------------------------------------------
                // Insert WeeklyPerformanceReport
                //------------------------------------------

                const string reportSql = @"

INSERT INTO WeeklyPerformanceReport
(
    Week,
    Month,
    Year,
    StaffName,
    Remarks,
    TotalScore,
    PaymentPercentage,
    SubmittedAt,
    AgreementId,
    ProviderId,
    HospitalId
)
VALUES
(
    @Week,
    @Month,
    @Year,
    @StaffName,
    @Remarks,
    @TotalScore,
    @PaymentPercentage,
    @SubmittedAt,
    @AgreementId,
    @ProviderId,
    @HospitalId
);

SELECT CAST(SCOPE_IDENTITY() AS INT);
";

                int wprId = await conn.QuerySingleAsync<int>(
                    reportSql,
                    wpr,
                    tran);

                //------------------------------------------
                // Insert WPREntries
                //------------------------------------------

                entry.CreatedOn = DateTime.Now;

                await conn.ExecuteAsync(@"

INSERT INTO WPREntries
(
AgreementId,
HospitalId,
WeekStart,
WeekEnd,
TotalScore,
CreatedOn,
ProviderId,
MonthNo,
YearNo,
WeekNo,
PerformanceGrade,
Remarks
)
VALUES
(
@AgreementId,
@HospitalId,
@WeekStart,
@WeekEnd,
@TotalScore,
@CreatedOn,
@ProviderId,
@MonthNo,
@YearNo,
@WeekNo,
@PerformanceGrade,
@Remarks
)

",
        entry,
        tran);

                //------------------------------------------
                // Insert Details
                //------------------------------------------

                foreach (var d in details)
                {
                    d.WPRId = wprId;
                }

                await conn.ExecuteAsync(@"

INSERT INTO WPRDetail
(
WPRId,
ParameterId,
ParameterName,
Score
)
VALUES
(
@WPRId,
@ParameterId,
@ParameterName,
@Score
)

",
        details,
        tran);

                //------------------------------------------
                // Commit
                //------------------------------------------

                tran.Commit();

                return wprId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // ══════════════════════════════════════════════════════
        // CMS REVIEW
        // ══════════════════════════════════════════════════════

        public async Task<List<WprListItemVM>> GetWprListAsync(
            int? hospitalId, int? providerId, string? status, int? month, int? year)
        {
            const string sql = @"
                SELECT w.Id, w.HospitalId, h.HospitalName, p.ProviderName,
                       w.Week, TRY_CAST(w.Month AS INT) AS Month, w.Year,
                       w.TotalScore, w.PaymentPercentage, w.Status,
                       w.SubmittedAt, w.VerifiedAt, w.LastEditedAt
                FROM WeeklyPerformanceReport w
                JOIN tbl_Hospitals h ON h.HospitalId = w.HospitalId
                LEFT JOIN tbl_Providers p ON p.ProviderId = w.ProviderId
                WHERE (@hospitalId IS NULL OR w.HospitalId = @hospitalId)
                  AND (@providerId IS NULL OR w.ProviderId = @providerId)
                  AND (@status     IS NULL OR w.Status     = @status)
                  AND (@month      IS NULL OR TRY_CAST(w.Month AS INT) = @month)
                  AND (@year       IS NULL OR w.Year       = @year)
                ORDER BY w.Year DESC, TRY_CAST(w.Month AS INT) DESC, w.Week DESC, w.Id DESC";

            using var conn = CreateConnection();
            return (await conn.QueryAsync<WprListItemVM>(sql,
                new { hospitalId, providerId, status, month, year })).ToList();
        }

        public async Task<WprReviewVM?> GetWprReviewAsync(int id)
        {
            using var conn = CreateConnection();

            var vm = await conn.QueryFirstOrDefaultAsync<WprReviewVM>(@"
                SELECT w.Id, w.HospitalId, h.HospitalName, p.ProviderName,
                       w.Week, TRY_CAST(w.Month AS INT) AS Month, w.Year,
                       w.TotalScore, w.PaymentPercentage, w.Status,
                       w.Remarks AS HospitalRemarks, w.SubmittedAt, w.VerifiedAt
                FROM WeeklyPerformanceReport w
                JOIN tbl_Hospitals h ON h.HospitalId = w.HospitalId
                LEFT JOIN tbl_Providers p ON p.ProviderId = w.ProviderId
                WHERE w.Id = @id", new { id });
            if (vm == null) return null;

            vm.Scores = (await conn.QueryAsync<WprScoreVM>(@"
                SELECT ParameterId, ParameterName, Score
                FROM WPRDetail WHERE WPRId = @id ORDER BY ParameterId", new { id })).ToList();

            vm.EditLog = (await conn.QueryAsync<WprEditLogVM>(@"
                SELECT l.ParameterName, l.OldScore, l.NewScore, l.OldTotal, l.NewTotal,
                       ISNULL(u.FullName, '') AS EditedByName, l.EditedByRole, l.EditedAt, l.Remarks
                FROM WPREditLog l
                LEFT JOIN Tbl_Users u ON u.UserId = l.EditedBy
                WHERE l.WPRId = @id
                ORDER BY l.EditedAt DESC, l.Id DESC", new { id })).ToList();

            return vm;
        }

        public async Task<(int Pending, int Verified)> GetStatusCountsAsync(int hospitalId)
        {
            using var conn = CreateConnection();
            var row = await conn.QuerySingleAsync(@"
                SELECT ISNULL(SUM(CASE WHEN Status = 'Pending'  THEN 1 ELSE 0 END), 0) AS Pending,
                       ISNULL(SUM(CASE WHEN Status = 'Verified' THEN 1 ELSE 0 END), 0) AS Verified
                FROM WeeklyPerformanceReport
                WHERE HospitalId = @hospitalId", new { hospitalId });
            return ((int)row.Pending, (int)row.Verified);
        }

        public async Task<List<int>> GetPendingIdsAsync(int hospitalId, int month, int year)
        {
            using var conn = CreateConnection();
            return (await conn.QueryAsync<int>(@"
                SELECT Id FROM WeeklyPerformanceReport
                WHERE HospitalId = @hospitalId AND Status = 'Pending'
                  AND TRY_CAST(Month AS INT) = @month AND Year = @year",
                new { hospitalId, month, year })).ToList();
        }

        // Saves the changed scores, the new total, a log row per changed score, and keeps the
        // WPREntries copy of the week (total + grade) in step.
        public async Task ApplyEditAsync(WprEditCommand cmd)
        {
            using var conn = CreateConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                foreach (var c in cmd.Changes)
                {
                    int rows = await conn.ExecuteAsync(@"
                        UPDATE WPRDetail SET Score = @NewScore
                        WHERE WPRId = @WprId AND ParameterId = @ParameterId",
                        new { cmd.WprId, c.ParameterId, c.NewScore }, tran);

                    if (rows == 0)
                        await conn.ExecuteAsync(@"
                            INSERT INTO WPRDetail (WPRId, ParameterId, ParameterName, Score)
                            VALUES (@WprId, @ParameterId, @ParameterName, @NewScore)",
                            new
                            {
                                cmd.WprId, c.ParameterId, c.NewScore,
                                // WPRDetail.ParameterName holds 50 characters
                                ParameterName = c.ParameterName.Length > 50 ? c.ParameterName[..50] : c.ParameterName
                            }, tran);

                    await conn.ExecuteAsync(@"
                        INSERT INTO WPREditLog
                            (WPRId, ParameterId, ParameterName, OldScore, NewScore, OldTotal, NewTotal,
                             EditedBy, EditedByRole, EditedAt, Remarks, DisputeId)
                        VALUES
                            (@WprId, @ParameterId, @ParameterName, @OldScore, @NewScore, @OldTotal, @NewTotal,
                             @EditorId, @EditorRole, GETDATE(), @Remarks, @DisputeId)",
                        new
                        {
                            cmd.WprId, c.ParameterId, c.ParameterName, c.OldScore, c.NewScore,
                            cmd.OldTotal, cmd.NewTotal, cmd.EditorId, cmd.EditorRole, cmd.Remarks, cmd.DisputeId
                        }, tran);
                }

                await conn.ExecuteAsync(@"
                    UPDATE WeeklyPerformanceReport
                    SET TotalScore = @NewTotal, PaymentPercentage = @NewPercentage,
                        LastEditedBy = @EditorId, LastEditedAt = GETDATE()
                    WHERE Id = @WprId",
                    new { cmd.NewTotal, cmd.NewPercentage, cmd.EditorId, cmd.WprId }, tran);

                var keys = await conn.QuerySingleAsync(@"
                    SELECT AgreementId, HospitalId, Week, Month, Year
                    FROM WeeklyPerformanceReport WHERE Id = @WprId",
                    new { cmd.WprId }, tran);

                await conn.ExecuteAsync(@"
                    UPDATE WPREntries
                    SET TotalScore = @NewTotal, PerformanceGrade = @NewGrade
                    WHERE AgreementId = @AgreementId AND HospitalId = @HospitalId
                      AND WeekNo = @Week AND MonthNo = TRY_CAST(@Month AS INT) AND YearNo = @Year",
                    new
                    {
                        cmd.NewTotal, cmd.NewGrade,
                        AgreementId = (int)keys.AgreementId, HospitalId = (int)keys.HospitalId,
                        Week = (int)keys.Week, Month = (string)keys.Month, Year = (int)keys.Year
                    }, tran);

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<int> VerifyAsync(IEnumerable<int> ids, int hospitalId, int userId)
        {
            var list = ids.ToList();
            if (list.Count == 0) return 0;

            using var conn = CreateConnection();
            return await conn.ExecuteAsync(@"
                UPDATE WeeklyPerformanceReport
                SET Status = 'Verified', VerifiedBy = @userId, VerifiedAt = GETDATE()
                WHERE Id IN @list AND HospitalId = @hospitalId AND Status = 'Pending'",
                new { list, hospitalId, userId });
        }
    }
}
