using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        //-------------------------------------------------------------
        // Generate Payment
        //-------------------------------------------------------------

        public async Task<bool> GeneratePayment(
      int agreementId,
      int hospitalId,
      int month,
      int year,
      int createdBy)
        {
            using var con = CreateConnection();

            var affected = await con.ExecuteAsync(

                "sp_GeneratePayment",

                new
                {
                    AgreementId = agreementId,
                    HospitalId = hospitalId,
                    MonthNo = month,
                    YearNo = year,
                    CreatedBy = createdBy
                },

                commandType: CommandType.StoredProcedure);

            return affected > 0;
        }

        //-------------------------------------------------------------
        // Pending Payments
        //-------------------------------------------------------------

        public async Task<List<PaymentMaster>> GetPendingPayments()
        {
            using var connection = CreateConnection();

            var data = await connection.QueryAsync<PaymentMaster>(
                @"SELECT *
                  FROM PaymentMaster
                  WHERE Status='Pending'
                  ORDER BY CreatedOn DESC");

            return data.ToList();
        }

        //-------------------------------------------------------------
        // Get Payment
        //-------------------------------------------------------------

        public async Task<PaymentMaster?> GetPaymentById(int paymentId)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<PaymentMaster>(
                @"SELECT *
                  FROM PaymentMaster
                  WHERE PaymentId=@PaymentId",
                new { PaymentId = paymentId });
        }

        //-------------------------------------------------------------
        // Approve Payment
        //-------------------------------------------------------------

        public async Task<bool> ApprovePayment(
     int paymentId,
     int approvedBy,
     string remarks)
        {
            using var conn = CreateConnection();

            int rows = await conn.ExecuteAsync(
                "sp_ApprovePayment",
                new
                {
                    PaymentId = paymentId,
                    ApprovedBy = approvedBy,
                    Remarks = remarks
                },
                commandType: CommandType.StoredProcedure);

            return rows > 0;
        }

        //-------------------------------------------------------------
        // Reject Payment
        //-------------------------------------------------------------

        public async Task<bool> RejectPayment(
     int paymentId,
     int rejectedBy,
     string remarks)
        {
            using var conn = CreateConnection();

            int rows = await conn.ExecuteAsync(
                "sp_RejectPayment",
                new
                {
                    PaymentId = paymentId,
                    RejectedBy = rejectedBy,
                    Remarks = remarks
                },
                commandType: CommandType.StoredProcedure);

            return rows > 0;
        }

        //-------------------------------------------------------------
        // Calculations
        //-------------------------------------------------------------

        public async Task<List<PaymentCalculation>> GetCalculations(int paymentId)
        {
            using var connection = CreateConnection();

            var data = await connection.QueryAsync<PaymentCalculation>(
                @"SELECT *
                  FROM PaymentCalculation
                  WHERE PaymentId=@PaymentId",
                new
                {
                    PaymentId = paymentId
                });

            return data.ToList();
        }

        //-------------------------------------------------------------
        // Documents
        //-------------------------------------------------------------

        public async Task<List<PaymentDocument>> GetDocuments(int paymentId)
        {
            using var connection = CreateConnection();

            var data = await connection.QueryAsync<PaymentDocument>(
                @"SELECT *
                  FROM PaymentDocuments
                  WHERE PaymentId=@PaymentId",
                new
                {
                    PaymentId = paymentId
                });

            return data.ToList();
        }

        //-------------------------------------------------------------
        // Upload Document
        //-------------------------------------------------------------

        public async Task UploadDocument(PaymentDocument document)
        {
            using var connection = CreateConnection();

            await connection.ExecuteAsync(
            @"INSERT INTO PaymentDocuments
            (
                PaymentId,
                DocumentType,
                FileName,
                FilePath,
                UploadedBy,
                UploadedOn
            )
            VALUES
            (
                @PaymentId,
                @DocumentType,
                @FileName,
                @FilePath,
                @UploadedBy,
                GETDATE()
            )",
            document);
        }
        //--------------------------------------------------
        // Approval History
        //--------------------------------------------------

        public async Task<List<PaymentApprovalLog>> GetApprovalHistory(int paymentId)
        {
            using var con = CreateConnection();

            var sql = @"
        SELECT
            Id,
            PaymentId,
            Status,
            Remarks,
            ActionBy,
            ActionDate
        FROM PaymentApprovalLog
        WHERE PaymentId = @PaymentId
        ORDER BY ActionDate DESC";

            var result = await con.QueryAsync<PaymentApprovalLog>(
                sql,
                new { PaymentId = paymentId });

            return result.ToList();
        }
        public async Task<List<PaymentMaster>> GetPayments(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AgreementId", agreementId);
            parameters.Add("@HospitalId", hospitalId);
            parameters.Add("@MonthNo", monthNo);
            parameters.Add("@YearNo", yearNo);
            parameters.Add("@Status", status);

            var result = await connection.QueryAsync<PaymentMaster>(
                "sp_GetPayments",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        //        public async Task<AgreementDetailsVM?> GetAgreementDetails(int agreementId)
        //        {
        //            using var con = CreateConnection();

        //            var sql = @"

        //SELECT

        //A.Id               AgreementId,

        //A.ProviderId,

        //P.ProviderName,

        //A.HospitalId,

        //H.HospitalName,

        //A.BedCount,

        //A.RatePerBed,

        //(A.BedCount*A.RatePerBed) ContractAmount,

        //A.IsActive

        //FROM ProviderHospitalAgreements A

        //INNER JOIN Providers P
        //ON A.ProviderId=P.ProviderId

        //INNER JOIN tbl_Hospitals H
        //ON A.HospitalId=H.HospitalId

        //WHERE A.Id=@agreementId";

        //            return await con.QueryFirstOrDefaultAsync<AgreementDetailsVM>(
        //                sql,
        //                new { agreementId });
        //        }
        public async Task<List<SelectListItem>> GetAgreementDropdown()
        {
            using var con = CreateConnection();

            string sql = @"

SELECT
    Id,
    CONCAT('Agreement - ', Id) AS AgreementName
FROM ProviderHospitalAgreements
WHERE IsActive = 1
ORDER BY Id";

            var data = await con.QueryAsync(sql);

            return data.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.AgreementName.ToString()
            }).ToList();
        }
        public async Task<GeneratePaymentVM> GetGeneratePaymentData(int hospitalId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<GeneratePaymentVM>(
                "sp_GetGeneratePaymentData",
                new
                {
                    HospitalId = hospitalId
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<PaymentCalculationVM> GetPaymentCalculation(
    int agreementId,
    int hospitalId,
    int monthNo,
    int yearNo,
    int bedOccupancy)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<PaymentCalculationVM>(
                "sp_GetPaymentCalculation",
                new
                {
                    AgreementId = agreementId,
                    HospitalId = hospitalId,
                    MonthNo = monthNo,
                    YearNo = yearNo,
                    BedOccupancy = bedOccupancy
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<AgreementDetailsVM> GetAgreementDetails(int agreementId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<AgreementDetailsVM>(
                "sp_GetAgreementDetails",
                new { AgreementId = agreementId },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<List<SelectListItem>> GetAgreementsByProvider(
      int hospitalId,
      int providerId)
        {
            using var con = CreateConnection();

            var result = await con.QueryAsync<SelectListItem>(
                "sp_GetAgreementsByProvider",
                new
                {
                    HospitalId = hospitalId,
                    ProviderId = providerId
                },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<bool> GeneratePayment(
    int agreementId,
    int hospitalId,
    int providerId,
    int monthNo,
    int yearNo,
    int bedOccupancy,
    int createdBy)
        {
            using var con = CreateConnection();

            return await con.ExecuteScalarAsync<bool>(
                "sp_GeneratePayment",
                new
                {
                    AgreementId = agreementId,
                    HospitalId = hospitalId,
                    ProviderId = providerId,
                    MonthNo = monthNo,
                    YearNo = yearNo,
                    BedOccupancy = bedOccupancy,
                    CreatedBy = createdBy
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}