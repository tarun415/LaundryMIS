using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using LaundryMIS.Models.LaudaryMis.Models;

namespace LaudaryMis.Services
{
    public interface IInvoiceService
    {
        Task<List<InvoiceMaster>> GetInvoiceList(
            int? agreementId,
            int? hospitalId,
            int? monthNo,
            int? yearNo,
            string status);

        Task<GenerateInvoiceVM> GetGenerateInvoiceData(
            int paymentId);

        Task<GenerateInvoiceResult> GenerateInvoice(
            GenerateInvoiceVM model);

        Task<InvoiceMaster> GetInvoiceDetails(
            int invoiceId);

        //Task<bool> ApproveInvoice(
        //    int invoiceId,
        //    string remarks,
        //    int approvedBy);

        //Task<bool> RejectInvoice(
        //    int invoiceId,
        //    string remarks,
        //    int rejectedBy);

        Task<bool> UploadInvoiceDocument(
            InvoiceDocument model);

        Task<InvoiceDocument> GetInvoiceDocument(int invoiceId);
        Task<byte[]> GenerateInvoicePdf(int invoiceId);
        Task<bool> GenerateAndSaveInvoicePdf(
    int invoiceId,
    int uploadedBy);
       
    }
}