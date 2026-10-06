using LaudaryMis.Helpers;
using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize]
    public class WarningLetterController : Controller
    {
        private readonly IWarningLetterService _warningLetterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHospitalService _hospitalService;
        private readonly IAgreementService _agreementService;
        private readonly IPaymentService _paymentService;
        private readonly IAccessGuard _guard;

        public WarningLetterController(
            IWarningLetterService warningLetterService,
            IWebHostEnvironment webHostEnvironment,
            IHospitalService hospitalService,
            IAgreementService agreementService,
            IPaymentService paymentService,
            IAccessGuard guard)
        {
            _guard = guard;
            _paymentService = paymentService;
            _warningLetterService = warningLetterService;
            _webHostEnvironment = webHostEnvironment;
            _hospitalService = hospitalService;
            _agreementService = agreementService;
        }
        public async Task<IActionResult> WarningLetterList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string? status)
        {
            // A hospital only lists its own letters, whatever the query string says
            if (User.IsInRole("Hospital"))
                hospitalId = User.HospitalId() ?? -1;

            // The list rows carry only the agreement, so they are limited by the user's own agreements
            var ownAgreements = await _guard.VisibleAgreementIdsAsync(User);

            var result = (await _warningLetterService.GetWarningLetterList(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status))
                .Where(x => ownAgreements == null || ownAgreements.Contains(x.AgreementId))
                .ToList();

            // The Agreement and Hospital filters used to be free-text ID boxes.
            // Typing a name bound to null, so the filter was silently dropped
            // and the full list came back unfiltered.
            var hospitals = await _hospitalService.GetAllAsync();
            var agreements = await _agreementService.GetAllAsync();

            // The filter lists only offer the signed-in hospital's / vendor's own contracts
            if (!User.IsAdmin())
            {
                agreements = agreements.Where(a => User.CanSee(a.HospitalId, a.ProviderId)).ToList();
                var ownHospitals = agreements.Select(a => a.HospitalId).ToHashSet();
                hospitals = hospitals.Where(h => h.HospitalId is int id && ownHospitals.Contains(id)).ToList();
            }

            ViewBag.Hospitals = hospitals;
            ViewBag.Agreements = agreements;

            return View(result);
        }
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GenerateWarningLetter(
    int paymentId)
        {
            if (!await CanSeePayment(paymentId)) return Forbid();

            var model = await _warningLetterService
                .GetGenerateWarningLetterData(paymentId);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Hospital,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateWarningLetter(
     GenerateWarningLetterVM model)
        {
            if (!await CanSeePayment(model.PaymentId)) return Forbid();

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

            model.CreatedBy = CurrentUserId();

            var result = await _warningLetterService
                .GenerateWarningLetter(model);

            if (result.Result == 1)
            {
                // Generate and Save PDF
                await _warningLetterService.GenerateWarningLetterPdf(
                    result.WarningId,
                    model.CreatedBy);

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

            if (!User.CanSee(warning.HospitalId, warning.ProviderId))
                return Forbid();

            return View(warning);
        }
        public async Task<IActionResult> PrintWarningLetter(
    int warningId)
        {
            if (!await CanSeeLetter(warningId)) return Forbid();

            var (document, filePath) = await ResolveDocument(warningId);

            if (filePath == null)
                return NotFound();

            return PhysicalFile(
                filePath,
                "application/pdf",
                document!.FileName);
        }
        public async Task<IActionResult> PreviewWarningLetter(int warningId)
        {
            if (!await CanSeeLetter(warningId)) return Forbid();

            var (document, filePath) = await ResolveDocument(warningId);

            if (filePath == null)
                return NotFound("Warning Letter PDF not found.");

            Response.Headers["Content-Disposition"] =
                $"inline; filename=\"{document!.FileName}\"";

            return PhysicalFile(
                filePath,
                "application/pdf");
        }
        public async Task<IActionResult> DownloadWarningLetter(
    int warningId)
        {
            if (!await CanSeeLetter(warningId)) return Forbid();

            var (document, filePath) = await ResolveDocument(warningId);

            if (filePath == null)
                return NotFound();

            return PhysicalFile(
                filePath,
                "application/pdf",
                document!.FileName);
        }

        // Returns the stored PDF for a warning letter, generating it only when
        // it has not been produced yet (or the file behind the row is missing).
        // Previously every print regenerated the PDF, which wrote a duplicate
        // file and inserted a duplicate document row on each click.
        private async Task<(WarningLetterDocument? Document, string? FilePath)>
            ResolveDocument(int warningId)
        {
            var document = await _warningLetterService
                .GetWarningLetterDocument(warningId);

            var filePath = ToWebRootPath(document);

            if (filePath != null)
                return (document, filePath);

            var pdf = await _warningLetterService
                .GenerateWarningLetterPdf(warningId, CurrentUserId());

            if (pdf == null)
                return (null, null);

            document = await _warningLetterService
                .GetWarningLetterDocument(warningId);

            return (document, ToWebRootPath(document));
        }

        private string? ToWebRootPath(WarningLetterDocument? document)
        {
            if (document == null || string.IsNullOrWhiteSpace(document.FilePath))
                return null;

            // TrimStart guards against a stored leading slash, which would make
            // Path.Combine discard the web root and return a rooted path.
            string path = Path.Combine(
                _webHostEnvironment.WebRootPath,
                document.FilePath.TrimStart('/', '\\'));

            return System.IO.File.Exists(path) ? path : null;
        }

        // A letter may be opened only by the hospital / vendor it is addressed to (admin: any)
        private async Task<bool> CanSeeLetter(int warningId)
        {
            var warning = await _warningLetterService.GetWarningLetterDetails(warningId);

            return warning != null && User.CanSee(warning.HospitalId, warning.ProviderId);
        }

        // A letter is generated from a payment, which must be the signed-in hospital's own
        private async Task<bool> CanSeePayment(int paymentId)
        {
            var payment = await _paymentService.GetPaymentById(paymentId);

            return payment != null && User.CanSee(payment.HospitalId, payment.ProviderId);
        }

        private int CurrentUserId()
        {
            return int.TryParse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                out var id) ? id : 0;
        }
    }
}