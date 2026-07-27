using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.ViewModels;
using LaundryMIS.Models.LaudaryMis.Models;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;

namespace LaudaryMis.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InvoiceController(IInvoiceService invoiceService, IWebHostEnvironment webHostEnvironment)
        {
            _invoiceService = invoiceService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> InvoiceList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status)
        {
            var result = await _invoiceService.GetInvoiceList(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status);

            return View(result);
        }
        public async Task<IActionResult> GenerateInvoice(
    int paymentId)
        {

            var model = await _invoiceService
                .GetGenerateInvoiceData(paymentId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateInvoice(
    GenerateInvoiceVM model)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _invoiceService
                    .GetGenerateInvoiceData(model.PaymentId);

                vm.InvoiceDate = model.InvoiceDate;

                vm.Remarks = model.Remarks;

                return View(vm);
            }

            model.CreatedBy = 1; //Later Claims

            var result = await _invoiceService.GenerateInvoice(model);

            if (result.Result == 1)
            {
                TempData["Success"] = "Invoice Generated Successfully.";

                return RedirectToAction(
                    "PrintInvoice",
                    new
                    {
                        invoiceId = result.InvoiceId
                    });
            }

            TempData["Error"] = result.ErrorMessage ?? "Unable to generate invoice.";

            var generateVM = await _invoiceService.GetGenerateInvoiceData(model.PaymentId);

            return View(generateVM);
            
        }
        public async Task<IActionResult> InvoiceDetails(
    int invoiceId)
        {
            var invoice = await _invoiceService
                .GetInvoiceDetails(invoiceId);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
        //    public async Task<IActionResult> ApproveInvoice(
        //int invoiceId)
        //    {
        //        bool result = await _invoiceService
        //            .ApproveInvoice(
        //                invoiceId,
        //                "Invoice Approved",
        //                1);

        //        if (result)
        //        {
        //            TempData["Success"] =
        //                "Invoice Approved.";
        //        }
        //        else
        //        {
        //            TempData["Error"] =
        //                "Unable to Approve Invoice.";
        //        }

        //        return RedirectToAction(nameof(InvoiceList));
        //    }
        //    public async Task<IActionResult> RejectInvoice(
        //int invoiceId)
        //    {
        //        bool result = await _invoiceService
        //            .RejectInvoice(
        //                invoiceId,
        //                "Invoice Rejected",
        //                1);

        //        if (result)
        //        {
        //            TempData["Success"] =
        //                "Invoice Rejected.";
        //        }
        //        else
        //        {
        //            TempData["Error"] =
        //                "Unable to Reject Invoice.";
        //        }

        //        return RedirectToAction(nameof(InvoiceList));
        //    }
        //      [HttpPost]
        //    public async Task<IActionResult> UploadInvoiceDocument(
        //InvoiceDocument model,
        //IFormFile file)
        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            TempData["Error"] = "Please select a file.";

        //            return RedirectToAction(
        //                "InvoiceDetails",
        //                new { invoiceId = model.InvoiceId });
        //        }

        //        string folder = Path.Combine(
        //            Directory.GetCurrentDirectory(),
        //            "wwwroot/InvoiceDocuments");

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }

        //        string fileName =
        //            Guid.NewGuid().ToString() +
        //            Path.GetExtension(file.FileName);

        //        string filePath =
        //            Path.Combine(folder, fileName);

        //        using (var stream = new FileStream(
        //            filePath,
        //            FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        model.FileName = file.FileName;

        //        model.FilePath =
        //            "/InvoiceDocuments/" + fileName;

        //        model.UploadedBy = 1;

        //        await _invoiceService
        //            .UploadInvoiceDocument(model);

        //        TempData["Success"] =
        //            "Invoice Uploaded Successfully.";

        //        return RedirectToAction(
        //            "InvoiceDetails",
        //            new { invoiceId = model.InvoiceId });
        //    }
        //public async Task<IActionResult> PreviewInvoice(int invoiceId)
        //{
        //    var document = await _invoiceService.GetInvoiceDocument(invoiceId);

        //    if (document == null)
        //        return NotFound("Invoice document not found.");

        //    string filePath = Path.Combine(
        //        _webHostEnvironment.WebRootPath,
        //        document.FilePath);

        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound("PDF not found.");

        //    Response.Headers.Add(
        //        "Content-Disposition",
        //        $"inline; filename={document.FileName}");

        //    return PhysicalFile(filePath, "application/pdf");
        //}
        //public async Task<IActionResult> PrintInvoice(int invoiceId)
        //{
        //    var invoice = await _invoiceService.GetInvoiceDetails(invoiceId);

        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    // Optional - Amount in Words
        //    ViewBag.AmountInWords = invoice.NetPayable.ToString("N2");
        //    //  ViewBag.AmountInWords = NumberToWords.Convert((long)invoice.NetPayable);

        //    return View(invoice);
        //}
        //public async Task<IActionResult> DownloadInvoice(int invoiceId)
        //{
        //    var invoice = await _invoiceService.GetInvoiceDetails(invoiceId);

        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    return new ViewAsPdf("PrintInvoice", invoice)
        //    {
        //        FileName = $"{invoice.InvoiceNo}.pdf",
        //        PageSize = Size.A4,
        //        PageOrientation = Orientation.Portrait,
        //        PageMargins = new Margins(10, 10, 10, 10),
        //        CustomSwitches = "--print-media-type --enable-local-file-access"
        //    };
        //}
        //public async Task<IActionResult> DownloadInvoice(int invoiceId)
        //{
        //    var pdf = await _invoiceService.GenerateInvoicePdf(invoiceId);

        //    if (pdf == null)
        //        return NotFound();

        //    return File(pdf, "application/pdf", $"Invoice_{invoiceId}.pdf");
        //}
        public async Task<IActionResult> PrintInvoice(int invoiceId)
        {
            var pdf = await _invoiceService.GenerateInvoicePdf(invoiceId);

            if (pdf == null)
                return NotFound();

            // Get invoice details to access InvoiceNo
            var invoice = await _invoiceService.GetInvoiceDetails(invoiceId);

            if (invoice == null)
                return NotFound();

            string fileName = $"{invoice.InvoiceNo}_{DateTime.Now:yyyyMMdd}_.pdf";

            return File(
                pdf,
                "application/pdf",
                fileName);
        }
    }
}