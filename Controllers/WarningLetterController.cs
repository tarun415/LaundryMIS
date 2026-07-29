using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class WarningLetterController : Controller
    {
        private readonly IWarningLetterService _warningLetterService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WarningLetterController(
            IWarningLetterService warningLetterService,
            IWebHostEnvironment webHostEnvironment)
        {
            _warningLetterService = warningLetterService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> WarningLetterList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string? status)
        {
            var result = await _warningLetterService.GetWarningLetterList(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status);

            return View(result);
        }
        public async Task<IActionResult> GenerateWarningLetter(
    int paymentId)
        {
            var model = await _warningLetterService
                .GetGenerateWarningLetterData(paymentId);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateWarningLetter(
     GenerateWarningLetterVM model)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _warningLetterService
                    .GetGenerateWarningLetterData(model.PaymentId);

                vm.WarningDate = model.WarningDate;
                vm.WarningLevel = model.WarningLevel;
                vm.Subject = model.Subject;
                vm.Reason = model.Reason;
                vm.Remarks = model.Remarks;

                return View(vm);
            }

            model.CreatedBy = Convert.ToInt32(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var result = await _warningLetterService
                .GenerateWarningLetter(model);

            if (result.Result == 1)
            {
                // Generate and Save PDF
                await _warningLetterService.GenerateWarningLetterPdf(result.WarningId);

                TempData["Success"] = "Warning Letter Generated Successfully.";

                return RedirectToAction(
                    "PreviewWarningLetter",
                    new
                    {
                        warningId = result.WarningId
                    });
            }

            TempData["Error"] = result.ErrorMessage;

            var generateVM = await _warningLetterService
                .GetGenerateWarningLetterData(model.PaymentId);

            return View(generateVM);
        }
        public async Task<IActionResult> WarningLetterDetails(
    int warningId)
        {
            var warning = await _warningLetterService
                .GetWarningLetterDetails(warningId);

            if (warning == null)
                return NotFound();

            return View(warning);
        }
        public async Task<IActionResult> PrintWarningLetter(
    int warningId)
        {
            var pdf = await _warningLetterService
                .GenerateWarningLetterPdf(warningId);

            if (pdf == null)
                return NotFound();

            var warning = await _warningLetterService
                .GetWarningLetterDetails(warningId);

            string fileName =
                $"{warning.WarningNo}_{DateTime.Now:yyyyMMdd}.pdf";

            return File(
                pdf,
                "application/pdf",
                fileName);
        }
        public async Task<IActionResult> PreviewWarningLetter(int warningId)
        {
            var document = await _warningLetterService
                .GetWarningLetterDocument(warningId);

            if (document == null)
                return NotFound("Warning Letter not found.");

            string filePath = Path.Combine(
                _webHostEnvironment.WebRootPath,
                document.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("PDF not found.");

            Response.Headers.Add(
                "Content-Disposition",
                $"inline; filename={document.FileName}");

            return PhysicalFile(
                filePath,
                "application/pdf");
        }
        public async Task<IActionResult> DownloadWarningLetter(
    int warningId)
        {
            var document = await _warningLetterService
                .GetWarningLetterDocument(warningId);

            if (document == null)
                return NotFound();

            string filePath = Path.Combine(
                _webHostEnvironment.WebRootPath,
                document.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(
                filePath,
                "application/pdf",
                document.FileName);
        }
    }
}