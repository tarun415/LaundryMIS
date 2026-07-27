using Dapper;
using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using LaundryMIS.Models.LaudaryMis.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IConfiguration _configuration;

        public InvoiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<List<InvoiceMaster>> GetInvoiceList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status)
        {
            using var con = CreateConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@AgreementId", agreementId);
            parameter.Add("@HospitalId", hospitalId);
            parameter.Add("@MonthNo", monthNo);
            parameter.Add("@YearNo", yearNo);
            parameter.Add("@Status", status);
            var result = await con.QueryAsync<InvoiceMaster>(
                "sp_GetInvoiceList",
                parameter,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<GenerateInvoiceVM> GetGenerateInvoiceData(
    int paymentId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<GenerateInvoiceVM>(
                "sp_GetGenerateInvoiceData",
                new
                {
                    PaymentId = paymentId
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<GenerateInvoiceResult> GenerateInvoice(
     GenerateInvoiceVM model)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<GenerateInvoiceResult>(
                "sp_GenerateInvoice",
                new
                {
                    model.PaymentId,
                    model.InvoiceDate,
                    model.Remarks,
                    CreatedBy = model.CreatedBy
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<InvoiceMaster> GetInvoiceDetails(
    int invoiceId)
        {
            using var con = CreateConnection();

            return await con.QueryFirstOrDefaultAsync<InvoiceMaster>(
                "sp_GetInvoiceDetails",
                new
                {
                    InvoiceId = invoiceId
                },
                commandType: CommandType.StoredProcedure);
        }
        //    public async Task<bool> ApproveInvoice(
        //int invoiceId,
        //string remarks,
        //int approvedBy)
        //    {
        //        using var con = CreateConnection();

        //        return await con.ExecuteScalarAsync<bool>(
        //            "sp_ApproveInvoice",
        //            new
        //            {
        //                InvoiceId = invoiceId,

        //                Remarks = remarks,

        //                ApprovedBy = approvedBy
        //            },
        //            commandType: CommandType.StoredProcedure);
        //    }
        //    public async Task<bool> RejectInvoice(
        //int invoiceId,
        //string remarks,
        //int rejectedBy)
        //    {
        //        using var con = CreateConnection();

        //        return await con.ExecuteScalarAsync<bool>(
        //            "sp_RejectInvoice",
        //            new
        //            {
        //                InvoiceId = invoiceId,

        //                Remarks = remarks,

        //                RejectedBy = rejectedBy
        //            },
        //            commandType: CommandType.StoredProcedure);
        //    }
        public async Task<bool> UploadInvoiceDocument(
        InvoiceDocument model)
        {
            using var con = CreateConnection();

            return await con.ExecuteScalarAsync<bool>(
                "sp_UploadInvoiceDocument",
                new
                {
                    model.InvoiceId,

                    model.FileName,

                    model.FilePath,

                    model.UploadedBy
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<InvoiceDocument> GetInvoiceDocument(int invoiceId)
        {
            using (var con = CreateConnection())
            {
                string sql = @"SELECT TOP 1 *
                       FROM InvoiceDocuments
                       WHERE InvoiceId=@InvoiceId
                       AND IsActive=1";

                return await con.QueryFirstOrDefaultAsync<InvoiceDocument>(
                    sql,
                    new { InvoiceId = invoiceId });
            }
        }


    }
}