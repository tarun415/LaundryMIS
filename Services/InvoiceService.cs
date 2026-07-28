using LaudaryMis.Documents;
using LaudaryMis.Models;
using LaudaryMis.Repository;
using LaudaryMis.ViewModels;
using LaundryMIS.Models.LaudaryMis.Models;
using QuestPDF.Fluent;

namespace LaudaryMis.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IWebHostEnvironment environment)
        {
            _invoiceRepository = invoiceRepository;
            _environment = environment;
        }
        public async Task<List<InvoiceMaster>> GetInvoiceList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status)
        {
            return await _invoiceRepository.GetInvoiceList(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status);
        }
        public async Task<GenerateInvoiceVM> GetGenerateInvoiceData(
    int paymentId)
        {
            return await _invoiceRepository.GetGenerateInvoiceData(
                paymentId);
        }
        public async Task<GenerateInvoiceResult> GenerateInvoice(
    GenerateInvoiceVM model)
        {
            return await _invoiceRepository.GenerateInvoice(model);
        }
        public async Task<InvoiceMaster> GetInvoiceDetails(
    int invoiceId)
        {
            return await _invoiceRepository.GetInvoiceDetails(invoiceId);
        }
        //    public async Task<bool> ApproveInvoice(
        //int invoiceId,
        //string remarks,
        //int approvedBy)
        //    {
        //        return await _invoiceRepository.ApproveInvoice(
        //            invoiceId,
        //            remarks,
        //            approvedBy);
        //    }

        //    public async Task<bool> RejectInvoice(
        //int invoiceId,
        //string remarks,
        //int rejectedBy)
        //    {
        //        return await _invoiceRepository.RejectInvoice(
        //            invoiceId,
        //            remarks,
        //            rejectedBy);
        //    }
        public async Task<bool> UploadInvoiceDocument(
        InvoiceDocument model)
        {
            return await _invoiceRepository.UploadInvoiceDocument(model);
        }
        public async Task<InvoiceDocument> GetInvoiceDocument(int invoiceId)
        {
            return await _invoiceRepository.GetInvoiceDocument(invoiceId);
        }
        public async Task<byte[]> GenerateInvoicePdf(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceDetails(invoiceId);

            if (invoice == null)
                return null;

            var document = new InvoicePdfDocument(invoice);

            return document.GeneratePdf();
        }
        public async Task<bool> GenerateAndSaveInvoicePdf(
    int invoiceId,
    int uploadedBy)
        {
            //Generate PDF
            byte[] pdf = await GenerateInvoicePdf(invoiceId);

            if (pdf == null)
                return false;

            //Invoice Details
            var invoice =
                await _invoiceRepository.GetInvoiceDetails(invoiceId);

            if (invoice == null)
                return false;

            //Folder

            string folder = Path.Combine(
                _environment.WebRootPath,
                "InvoiceDocuments");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            //File Name

            string fileName =
                $"{invoice.InvoiceNo}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            string fullPath =
                Path.Combine(folder, fileName);

            //Save PDF

            await File.WriteAllBytesAsync(fullPath, pdf);

            //Save DB

            InvoiceDocument document =
                new InvoiceDocument
                {
                    InvoiceId = invoice.InvoiceId,
                    FileName = fileName,
                    FilePath = "/InvoiceDocuments/" + fileName,
                    UploadedBy = uploadedBy
                };

            await _invoiceRepository
                .UploadInvoiceDocument(document);

            return true;
        }
    }
}