using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;
using static LaudaryMis.ViewModels.CommonVM;

namespace LaudaryMis.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;

        public ReportRepository(IConfiguration config, IDbConnection db)
        {
            _config = config;
            _db = db;
        }

        public async Task<List<DeliverySummaryVM>>
GetDeliverySummaryReport()
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliverySummaryVM>(
                    "sp_GetDeliverySummaryReport",
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<List<DeliveryHistoryVM>>
        GetDeliveryHistory(int pickupId)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliveryHistoryVM>(
                    "sp_GetDeliveryHistory",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<List<WeeklyDeliveryReport>> WeeklyDeliveryReport(
     DateTime fromDate,
     DateTime toDate,
     int? hospitalId = null,
     int? providerId = null)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            // sp_GetWeeklyReport adds up every hospital. For one hospital / vendor the same
            // query runs here with the extra filter, so no stored procedure has to change.
            if (hospitalId != null || providerId != null)
            {
                var scoped = await con.QueryAsync<WeeklyDeliveryReport>(@"
                    SELECT
                        CAST(LP.PickupDateTime AS DATE) ReportDate,
                        COUNT(DISTINCT LP.PickupId) TotalPickups,
                        SUM(LPI.CollectedQty) TotalCollectedQty,
                        SUM(ISNULL(LPI.DeliveredQty,0)) TotalDeliveredQty,
                        SUM(ISNULL(LPI.PendingQty,0)) TotalPendingQty,
                        COUNT(DISTINCT CASE WHEN LP.Status='Verified' THEN LP.PickupId END) FullyDeliveredCount,
                        COUNT(DISTINCT CASE WHEN LP.Status='Partial Delivered' THEN LP.PickupId END) PartialDeliveredCount
                    FROM LaundryPickup LP
                    INNER JOIN LaundryPickupItems LPI ON LP.PickupId = LPI.PickupId
                    WHERE CAST(LP.PickupDateTime AS DATE) BETWEEN @FromDate AND @ToDate
                      AND (@HospitalId IS NULL OR LP.HospitalId = @HospitalId)
                      AND (@ProviderId IS NULL OR LP.ProviderId = @ProviderId)
                    GROUP BY CAST(LP.PickupDateTime AS DATE)
                    ORDER BY CAST(LP.PickupDateTime AS DATE)",
                    new
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        HospitalId = hospitalId,
                        ProviderId = providerId
                    });

                return scoped.ToList();
            }

            var result =
                await con.QueryAsync<WeeklyDeliveryReport>(
                    "sp_GetWeeklyReport",
                    new
                    {
                        FromDate = fromDate,
                        ToDate = toDate
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<List<MonthlyReportVM>>
GetMonthlyReport(
int year,
int month,
int? hospitalId = null,
int? providerId = null)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            // sp_GetMonthlyReport adds up every hospital. For one hospital / vendor the same
            // query runs here with the extra filter, so no stored procedure has to change.
            if (hospitalId != null || providerId != null)
            {
                var scoped = await con.QueryAsync<MonthlyReportVM>(@"
                    SELECT
                        DATENAME(MONTH, DATEFROMPARTS(@Year, @Month, 1)) + ' ' + CAST(@Year AS VARCHAR) AS MonthName,
                        COUNT(DISTINCT LP.PickupId) AS TotalPickups,
                        SUM(LP.TotalCollectedQty) AS CollectedQty,
                        ISNULL(SUM(D.DeliveredQty), 0) AS DeliveredQty,
                        SUM(LP.TotalCollectedQty) - ISNULL(SUM(D.DeliveredQty), 0) AS PendingQty,
                        COUNT(DISTINCT CASE WHEN LP.Status = 'Delivered' THEN LP.PickupId END) AS FullyDelivered,
                        COUNT(DISTINCT CASE WHEN LP.Status = 'Partial Delivered' THEN LP.PickupId END) AS PartialDelivered
                    FROM LaundryPickup LP
                    LEFT JOIN (
                        SELECT DC.PickupId, SUM(DCI.DeliveredQty) AS DeliveredQty
                        FROM DeliveryChallan DC
                        JOIN DeliveryChallanItems DCI ON DC.DeliveryId = DCI.DeliveryId
                        GROUP BY DC.PickupId
                    ) D ON D.PickupId = LP.PickupId
                    WHERE YEAR(LP.PickupDateTime) = @Year
                      AND MONTH(LP.PickupDateTime) = @Month
                      AND (@HospitalId IS NULL OR LP.HospitalId = @HospitalId)
                      AND (@ProviderId IS NULL OR LP.ProviderId = @ProviderId)",
                    new
                    {
                        Year = year,
                        Month = month,
                        HospitalId = hospitalId,
                        ProviderId = providerId
                    });

                return scoped.ToList();
            }

            var result =
                await con.QueryAsync<MonthlyReportVM>(
                    "sp_GetMonthlyReport",
                    new
                    {
                        Year = year,
                        Month = month
                    },
                    commandType:
                    CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<List<MonthlyPickupDetailVM>>
GetMonthlyPickupDetails(
int month,
int year)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<MonthlyPickupDetailVM>(
                    "sp_GetMonthlyPickupDetails",
                    new
                    {
                        Month = month,
                        Year = year
                    },
                    commandType:
                    CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<List<PendingLinenReportVM>>
GetPendingLinenReport()
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<PendingLinenReportVM>(
                    "sp_GetPendingLinenReport",
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<DeliveryAgingSummaryVM>
  GetDeliveryAgingSummary()
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            return await con.QueryFirstOrDefaultAsync<DeliveryAgingSummaryVM>(
                "sp_GetDeliveryAgingSummary",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<DeliveryAgingReportVM>>
        GetDeliveryAgingReport()
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliveryAgingReportVM>(
                    "sp_GetDeliveryAgingReport",
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<List<DeliveryAgingItemVM>>
        GetDeliveryAgingDetailItems(int pickupId)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

            var result =
                await con.QueryAsync<DeliveryAgingItemVM>(
                    "sp_GetDeliveryAgingDetailItems",
                    new
                    {
                        PickupId = pickupId
                    },
                    commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
    }
}