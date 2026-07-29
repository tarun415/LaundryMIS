using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repository;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class WarningLetterRepository : IWarningLetterRepository
    {
        private readonly IConfiguration _configuration;

        public WarningLetterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<List<WarningLetterMaster>> GetWarningLetterList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string? status)
        {
            using var con = CreateConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AgreementId", agreementId);
            parameter.Add("@HospitalId", hospitalId);
            parameter.Add("@MonthNo", monthNo);
            parameter.Add("@YearNo", yearNo);
            parameter.Add("@Status", status);

            var result = await con.QueryAsync<WarningLetterMaster>(
                "sp_GetWarningLetterList",
                parameter,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<GenerateWarningLetterVM> GetGenerateWarningLetterData(
    int paymentId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<GenerateWarningLetterVM>(
                "sp_GetGenerateWarningLetterData",
                new
                {
                    PaymentId = paymentId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<GenerateWarningLetterResult> GenerateWarningLetter(
    GenerateWarningLetterVM model)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<GenerateWarningLetterResult>(
                "sp_GenerateWarningLetter",
                new
                {
                    model.PaymentId,
                    model.WarningDate,
                    model.WarningLevel,
                    model.Subject,
                    model.Reason,
                    model.Remarks,
                    CreatedBy = model.CreatedBy
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<WarningLetterMaster> GetWarningLetterDetails(
    int warningId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<WarningLetterMaster>(
                "sp_GetWarningLetterDetails",
                new
                {
                    WarningId = warningId
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> UploadWarningLetterDocument(
    WarningLetterDocument model)
        {
            using var con = CreateConnection();

            return await con.ExecuteScalarAsync<bool>(
                "sp_UploadWarningLetterDocument",
                new
                {
                    model.WarningId,
                    model.FileName,
                    model.FilePath,
                    model.UploadedBy
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<WarningLetterDocument> GetWarningLetterDocument(
    int warningId)
        {
            using var con = CreateConnection();

            string sql = @"
    SELECT TOP 1 *
    FROM WarningLetterDocuments
    WHERE WarningId=@WarningId
      AND IsActive=1";

            return await con.QueryFirstOrDefaultAsync<WarningLetterDocument>(
                sql,
                new
                {
                    WarningId = warningId
                });
        }
    }

}
