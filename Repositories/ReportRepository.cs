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
     DateTime toDate)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

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
int month)
        {
            using var con =
                new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

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
    }
}