using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class MonthlyBillRepository : IMonthlyBillRepository
    {
        private readonly string _connStr;

        public MonthlyBillRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");
        }

        private IDbConnection Conn() => new SqlConnection(_connStr);

        // ──────────────────────────────────────────────────────
        // WPR Average Score for a Hospital+Month+Year
        // WeeklyPerformanceReport.Month nvarchar hai (e.g. "May")
        // isliye DATENAME se match kar rahe hain
        // ──────────────────────────────────────────────────────
        public async Task<(decimal? AvgScore, int WeeksCount)>
            GetWPRAvgScoreAsync(int hospitalId, int month, int year)
        {
            const string sql = @"
                SELECT
                    AVG(CAST(w.TotalScore AS DECIMAL(6,2))) AS AvgScore,
                    COUNT(DISTINCT w.Week)                  AS WeeksCount
                FROM WeeklyPerformanceReport w
                INNER JOIN ProviderHospitalAgreements a
                    ON w.AgreementId = a.Id
                WHERE a.HospitalId = @hospitalId
                  AND a.IsActive   = 1
                  AND w.Year       = @year
                  AND w.Month      = DATENAME(MONTH,
                        DATEFROMPARTS(@year, @month, 1))";

            using var conn = Conn();
            var row = await conn.QuerySingleAsync(sql,
                new { hospitalId, month, year });

            int weeks = (int)(row.WeeksCount ?? 0);
            decimal? avg = weeks > 0 ? (decimal?)row.AvgScore : null;
            return (avg, weeks);
        }

        // ──────────────────────────────────────────────────────
        // Agreement Info — form pre-fill ke liye
        // ──────────────────────────────────────────────────────
        public async Task<MonthlyBillVM> GetAgreementInfoAsync(int hospitalId)
        {
            const string sql = @"
                SELECT
                    a.Id            AS AgreementId,
                    a.HospitalId,
                    a.BedCount      AS SanctionedBeds,
                    a.RatePerBed    AS RatePerBedPerYear,
                    h.HospitalName,
                    h.District,
                    a.AgreementFile AS ContractNo
                FROM ProviderHospitalAgreements a
                JOIN tbl_Hospitals h
                    ON a.HospitalId = h.HospitalId
                WHERE a.HospitalId = @hospitalId
                  AND a.IsActive   = 1";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<MonthlyBillVM>(
                sql, new { hospitalId });
        }

        // ──────────────────────────────────────────────────────
        // Insert Bill — returns new Id
        // ──────────────────────────────────────────────────────
        public async Task<int> InsertBillAsync(MonthlyBill bill)
        {
            const string sql = @"
                INSERT INTO MonthlyBills (
                    AgreementId, HospitalId, ProviderId, BillingMonth, BillingYear,
                    SanctionedBeds, RatePerBedPerYear, GSTPercent,
                    WPRAvgScore, WPRWeeksConsidered,
                    IsScoreOverridden, OverrideReason,
                    AnnualValueExGST, AnnualValueInGST,
                    MonthlyGrossAmount, PaymentBandPercent,
                    BasePayableAmount, TDSPercent, TDSAmount,
                    AdditionalDeductions, DeductionRemarks,
                    NetPayableAmount, Status, CreatedBy, CreatedAt
                ) VALUES (
                    @AgreementId, @HospitalId, @ProviderId, @BillingMonth, @BillingYear,
                    @SanctionedBeds, @RatePerBedPerYear, @GSTPercent,
                    @WPRAvgScore, @WPRWeeksConsidered,
                    @IsScoreOverridden, @OverrideReason,
                    @AnnualValueExGST, @AnnualValueInGST,
                    @MonthlyGrossAmount, @PaymentBandPercent,
                    @BasePayableAmount, @TDSPercent, @TDSAmount,
                    @AdditionalDeductions, @DeductionRemarks,
                    @NetPayableAmount, @Status, @CreatedBy, @CreatedAt
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = Conn();
            return await conn.QuerySingleAsync<int>(sql, bill);
        }

        // ──────────────────────────────────────────────────────
        // Update Bill (Draft edit ya rejection ke baad re-edit)
        // ──────────────────────────────────────────────────────
        public async Task UpdateBillAsync(MonthlyBill bill)
        {
            const string sql = @"
                UPDATE MonthlyBills SET
                    WPRAvgScore          = @WPRAvgScore,
                    WPRWeeksConsidered   = @WPRWeeksConsidered,
                    IsScoreOverridden    = @IsScoreOverridden,
                    OverrideReason       = @OverrideReason,
                    AnnualValueExGST     = @AnnualValueExGST,
                    AnnualValueInGST     = @AnnualValueInGST,
                    MonthlyGrossAmount   = @MonthlyGrossAmount,
                    PaymentBandPercent   = @PaymentBandPercent,
                    BasePayableAmount    = @BasePayableAmount,
                    TDSAmount            = @TDSAmount,
                    AdditionalDeductions = @AdditionalDeductions,
                    DeductionRemarks     = @DeductionRemarks,
                    NetPayableAmount     = @NetPayableAmount,
                    Status               = @Status
                WHERE Id = @Id";

            using var conn = Conn();
            await conn.ExecuteAsync(sql, bill);
        }

        // ──────────────────────────────────────────────────────
        // Get Bill By Id
        // ──────────────────────────────────────────────────────
        public async Task<MonthlyBill?> GetBillByIdAsync(int billId)
        {
            const string sql =
                "SELECT * FROM MonthlyBills WHERE Id = @billId";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<MonthlyBill>(
                sql, new { billId });
        }

        // ──────────────────────────────────────────────────────
        // Get Bill By Hospital + Month + Year (duplicate check)
        // ──────────────────────────────────────────────────────
        public async Task<MonthlyBill?> GetBillByHospitalMonthAsync(
            int hospitalId, int month, int year)
        {
            const string sql = @"
                SELECT * FROM MonthlyBills
                WHERE HospitalId   = @hospitalId
                  AND BillingMonth = @month
                  AND BillingYear  = @year";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<MonthlyBill>(
                sql, new { hospitalId, month, year });
        }

        // ──────────────────────────────────────────────────────
        // Update Status (Submit / CMSApprove / CMSReject)
        // ──────────────────────────────────────────────────────
        public async Task UpdateBillStatusAsync(
            int billId, string newStatus, int actionBy, string? remarks,
            DateTime? cmsActionAt = null, int? cmsActionBy = null)
        {
            const string sql = @"
                UPDATE MonthlyBills SET
                    Status       = @newStatus,
                    SubmittedAt  = CASE
                                     WHEN @newStatus = 'Submitted'
                                     THEN GETDATE()
                                     ELSE SubmittedAt
                                   END,
                    CMSActionBy  = ISNULL(@cmsActionBy, CMSActionBy),
                    CMSActionAt  = ISNULL(@cmsActionAt, CMSActionAt),
                    CMSRemarks   = ISNULL(@remarks, CMSRemarks)
                WHERE Id = @billId";

            using var conn = Conn();
            await conn.ExecuteAsync(sql,
                new { billId, newStatus, remarks, cmsActionAt, cmsActionBy });
        }

        // ──────────────────────────────────────────────────────
        // Insert Workflow Log Entry
        // ──────────────────────────────────────────────────────
        public async Task InsertWorkflowLogAsync(BillWorkflowLog log)
        {
            const string sql = @"
                INSERT INTO BillWorkflowLog
                    (BillId, FromStatus, ToStatus, ActionBy, ActionAt, Remarks)
                VALUES
                    (@BillId, @FromStatus, @ToStatus, @ActionBy, @ActionAt, @Remarks)";

            using var conn = Conn();
            await conn.ExecuteAsync(sql, log);
        }

        // ──────────────────────────────────────────────────────
        // Get All Bills (Admin / CMS list)
        // ──────────────────────────────────────────────────────
        public async Task<IEnumerable<BillListItemVM>> GetAllBillsAsync(
            string? status = null, int? hospitalId = null)
        {
            const string sql = @"
                SELECT
                    b.Id                AS BillId,
                    h.HospitalName,
                    h.District,
                    b.BillingMonth,
                    b.BillingYear,
                    b.WPRAvgScore,
                    b.PaymentBandPercent,
                    b.NetPayableAmount,
                    b.Status,
                    b.IsScoreOverridden,
                    b.CreatedAt
                FROM MonthlyBills b
                JOIN tbl_Hospitals h
                    ON b.HospitalId = h.HospitalId
                WHERE (@status     IS NULL OR b.Status     = @status)
                  AND (@hospitalId IS NULL OR b.HospitalId = @hospitalId)
                ORDER BY
                    b.BillingYear  DESC,
                    b.BillingMonth DESC,
                    h.HospitalName ASC";

            using var conn = Conn();
            return await conn.QueryAsync<BillListItemVM>(
                sql, new { status, hospitalId });
        }

        // ──────────────────────────────────────────────────────
        // Get Hospital's Own Bills
        // ──────────────────────────────────────────────────────
        public async Task<IEnumerable<BillListItemVM>> GetHospitalBillsAsync(
            int hospitalId)
        {
            return await GetAllBillsAsync(null, hospitalId);
        }

        // ──────────────────────────────────────────────────────
        // Get Workflow History for a Bill
        // ──────────────────────────────────────────────────────
        public async Task<IEnumerable<BillWorkflowLogVM>> GetWorkflowLogAsync(
            int billId)
        {
            const string sql = @"
                SELECT
                    wl.FromStatus,
                    wl.ToStatus,
                    u.FullName  AS ActionByName,
                    wl.ActionAt,
                    wl.Remarks
                FROM BillWorkflowLog wl
                JOIN Tbl_Users u
                    ON wl.ActionBy = u.UserId
                WHERE wl.BillId = @billId
                ORDER BY wl.ActionAt ASC";

            using var conn = Conn();
            return await conn.QueryAsync<BillWorkflowLogVM>(
                sql, new { billId });
        }

        public async Task<MonthlyBillVM?> GetAgreementInfoByProviderHospitalAsync(
    int providerId, int hospitalId)
        {
            const string sql = @"
        SELECT
            a.Id            AS AgreementId,
            a.HospitalId,
            a.ProviderId,
            a.BedCount      AS SanctionedBeds,
            a.RatePerBed    AS RatePerBedPerYear,
            h.HospitalName,
            h.District,
            a.AgreementFile AS ContractNo,
            p.ProviderName
        FROM ProviderHospitalAgreements a
        JOIN tbl_Hospitals h ON a.HospitalId = h.HospitalId
        JOIN tbl_Providers p ON a.ProviderId  = p.ProviderId
        WHERE a.ProviderId  = @providerId
          AND a.HospitalId  = @hospitalId
          AND a.IsActive    = 1";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<MonthlyBillVM>(
                sql, new { providerId, hospitalId });
        }

        // Provider + Hospital + Month se bill dhundo
        public async Task<MonthlyBill?> GetBillByProviderHospitalMonthAsync(
            int providerId, int hospitalId, int month, int year)
        {
            const string sql = @"
        SELECT * FROM MonthlyBills
        WHERE ProviderId   = @providerId
          AND HospitalId   = @hospitalId
          AND BillingMonth = @month
          AND BillingYear  = @year";

            using var conn = Conn();
            return await conn.QuerySingleOrDefaultAsync<MonthlyBill>(
                sql, new { providerId, hospitalId, month, year });
        }

        // Provider ke sab bills
        public async Task<IEnumerable<BillListItemVM>> GetProviderBillsAsync(
            int providerId)
        {
            const string sql = @"
        SELECT
            b.Id                AS BillId,
            h.HospitalName,
            h.District,
            p.ProviderName,
            b.ProviderId,
            b.BillingMonth,
            b.BillingYear,
            b.WPRAvgScore,
            b.PaymentBandPercent,
            b.NetPayableAmount,
            b.Status,
            b.IsScoreOverridden,
            b.CreatedAt
        FROM MonthlyBills b
        JOIN tbl_Hospitals h ON b.HospitalId  = h.HospitalId
        JOIN tbl_Providers p ON b.ProviderId   = p.ProviderId
        WHERE b.ProviderId = @providerId
        ORDER BY b.BillingYear DESC, b.BillingMonth DESC";

            using var conn = Conn();
            return await conn.QueryAsync<BillListItemVM>(sql, new { providerId });
        }

        // Hospital ke verify-pending bills
        public async Task<IEnumerable<BillListItemVM>> GetBillsForHospitalVerifyAsync(
            int hospitalId)
        {
            const string sql = @"
        SELECT
            b.Id                AS BillId,
            h.HospitalName,
            h.DistrictId,
            p.ProviderName,
            b.ProviderId,
            b.BillingMonth,
            b.BillingYear,
            b.WPRAvgScore,
            b.PaymentBandPercent,
            b.NetPayableAmount,
            b.Status,
            b.IsScoreOverridden,
            b.CreatedAt
        FROM MonthlyBills b
        JOIN tbl_Hospitals h ON b.HospitalId = h.HospitalId
        JOIN tbl_Providers p ON b.ProviderId  = p.ProviderId
        WHERE b.HospitalId = @hospitalId
        ORDER BY b.BillingYear DESC, b.BillingMonth DESC";

            using var conn = Conn();
            return await conn.QueryAsync<BillListItemVM>(sql, new { hospitalId });
        }

        // Hospital approve / reject
        public async Task UpdateHospitalActionAsync(
            int billId, bool approve, int hospitalUserId, string? remarks)
        {
            string newStatus = approve ? "HospitalApproved" : "HospitalRejected";

            const string sql = @"
        UPDATE MonthlyBills SET
            Status           = @newStatus,
            HospitalActionBy = @hospitalUserId,
            HospitalActionAt = GETDATE(),
            HospitalRemarks  = @remarks
        WHERE Id = @billId";

            using var conn = Conn();
            await conn.ExecuteAsync(sql,
                new { billId, newStatus, hospitalUserId, remarks });
        }

    }
}