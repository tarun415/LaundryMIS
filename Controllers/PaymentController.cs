using LaudaryMis.Models;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IProviderProfileService _profileService;
    private readonly LaudaryMis.Helpers.PortalCalendar _calendar;

    public PaymentController(
        IPaymentService paymentService,
        IProviderProfileService profileService,
        LaudaryMis.Helpers.PortalCalendar calendar)
    {
        _paymentService = paymentService;
        _profileService = profileService;
        _calendar = calendar;
    }

    //-------------------------------------------------------
    // Payment List
    //-------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> PaymentList(
        int? agreementId,
        int? hospitalId,
        int? monthNo,
        int? yearNo,
        string status)
    {
        int? providerId = null;

        // Hospital / Provider sirf apni payments dekhein; Admin filter chuna sakta hai
        if (User.IsInRole("Hospital"))
            hospitalId = GetClaimId("HospitalId");
        else if (User.IsInRole("Provider"))
            providerId = GetClaimId("ProviderId");
        else if (!User.IsInRole("Admin"))
            return Forbid();

        var model = await _paymentService.GetPayments(
            agreementId,
            hospitalId,
            providerId,
            monthNo,
            yearNo,
            status);

        return View(model);
    }

    //-------------------------------------------------------
    // Generate Payment
    //-------------------------------------------------------

    [Authorize(Roles = "Hospital")]
    public async Task<IActionResult> GeneratePayment()
    {
        int hospitalId = Convert.ToInt32(
            User.Claims.First(x => x.Type == "HospitalId").Value);

        var model = await _paymentService.GetGeneratePaymentData(hospitalId);

        model.MonthNo = DateTime.Now.Month;
        model.YearNo = DateTime.Now.Year;

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Hospital")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GeneratePayment(
     GeneratePaymentVM model)
    {
        // Hospital comes from the login, never from the posted form.
        model.HospitalId = GetClaimId("HospitalId");

        // Agreement must belong to this hospital; amounts are recalculated
        // on the server so posted values cannot be tampered with.
        var agreement = await _paymentService.GetAgreementDetails(model.AgreementId);
        if (agreement == null || agreement.HospitalId != model.HospitalId)
            return Forbid();

        // Portal shuru hone se pehle ke mahine offline pay ho chuke hain
        if (_calendar.IsBeforeStart(model.MonthNo, model.YearNo))
            ModelState.AddModelError("", _calendar.BeforeStartMessage("Payment"));

        var calc = await _paymentService.GetPaymentCalculation(
            model.AgreementId, model.HospitalId,
            model.MonthNo, model.YearNo, model.BedOccupancy);
        if (calc == null)
            ModelState.AddModelError("", "The payment could not be calculated.");
        else
        {
            model.ProviderId = agreement.ProviderId;
            model.BedCount = agreement.BedCount;
            model.RatePerBed = calc.RatePerBed;
            model.MonthlyBill = calc.MonthlyBill;
            model.AverageScore = calc.AverageScore;
            model.PaymentPercentage = calc.PaymentPercentage;
            model.GrossPayable = calc.GrossPayable;
            model.GSTPercentage = calc.GSTPercentage;
            model.GSTAmount = calc.GSTAmount;
            model.InvoiceAmount = calc.InvoiceAmount;
            model.TDSPercentage = calc.TDSPercentage;
            model.TDSAmount = calc.TDSAmount;
            model.NetPayable = calc.NetPayable;
        }

        if (!ModelState.IsValid)
        {
            var vm = await _paymentService
                .GetGeneratePaymentData(model.HospitalId);

            vm.MonthNo = model.MonthNo;
            vm.YearNo = model.YearNo;

            return View(vm);
        }

        bool result =
            await _paymentService.GeneratePayment(model);

        if (result)
        {
            TempData["Success"] =
                "Payment Generated Successfully.";

            return RedirectToAction(nameof(PaymentList));
        }

        TempData["Error"] =
            "Payment already generated.";

        var data =
            await _paymentService.GetGeneratePaymentData(
                model.HospitalId);

        data.MonthNo = model.MonthNo;
        data.YearNo = model.YearNo;

        return View(data);
    }

    //-------------------------------------------------------
    // Payment Details
    //-------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> PaymentDetails(int paymentId)
    {
        var payment = await _paymentService.GetPaymentById(paymentId);

        if (payment == null)
            return NotFound();

        if (!CanAccess(payment))
            return Forbid();

        var vm = new PaymentDetailsVM
        {
            Payment = payment,
            Documents = await _paymentService.GetDocuments(paymentId),
            History = await _paymentService.GetApprovalHistory(paymentId),
            LabourLicence = await _profileService.GetLabourLicenceStatusAsync(payment.ProviderId)
        };

        return View(vm);
    }

    //-------------------------------------------------------
    // Approve (Admin / CMS)
    //-------------------------------------------------------

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePayment(
     int paymentId,
     string? remarks)
    {
        var payment = await _paymentService.GetPaymentById(paymentId);
        if (payment == null)
            return NotFound();

        // Contract: no payment is released until the provider's labour licence is submitted
        var licence = await _profileService.GetLabourLicenceStatusAsync(payment.ProviderId);
        if (!licence.IsValid)
        {
            TempData["Error"] =
                $"This payment cannot be approved: the provider's Labour Licence {licence.Problem}. " +
                "Under the contract no payment is released until a valid licence is submitted.";
            return RedirectToAction(nameof(PaymentDetails), new { paymentId });
        }

        bool result = await _paymentService.ApprovePayment(
            paymentId,
            GetUserId(),
            remarks ?? "");

        if (result)
            TempData["Success"] = "Payment Approved Successfully.";
        else
            TempData["Error"] = "Only a 'Pending' payment can be approved.";

        return RedirectToAction(nameof(PaymentDetails),
            new { paymentId });
    }

    //-------------------------------------------------------
    // Reject (Admin / CMS)
    //-------------------------------------------------------

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectPayment(int paymentId, string? remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
        {
            TempData["Error"] = "A reason is required when rejecting.";
            return RedirectToAction(nameof(PaymentDetails), new { paymentId });
        }

        bool result = await _paymentService.RejectPayment(
            paymentId, GetUserId(), remarks);

        if (result)
            TempData["Success"] = "Payment Rejected Successfully.";
        else
            TempData["Error"] = "Only a 'Pending' payment can be rejected.";

        return RedirectToAction(nameof(PaymentDetails),
            new { paymentId });
    }

    //-------------------------------------------------------
    // Upload Document
    //-------------------------------------------------------

    private static readonly string[] AllowedDocExtensions =
        { ".pdf", ".jpg", ".jpeg", ".png" };

    private const long MaxDocSize = 10 * 1024 * 1024; // 10 MB

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadPaymentDocument(
        int paymentId,
        string documentType,
        IFormFile? file)
    {
        var payment = await _paymentService.GetPaymentById(paymentId);

        if (payment == null)
            return NotFound();

        if (!CanAccess(payment))
            return Forbid();

        string ext = Path.GetExtension(file?.FileName ?? "").ToLowerInvariant();

        string? error = await LaudaryMis.Helpers.UploadValidator.ValidateAsync(
            file, AllowedDocExtensions, MaxDocSize);

        if (error == null && string.IsNullOrWhiteSpace(documentType))
            error = "Select a document type.";

        if (error != null)
        {
            TempData["Error"] = error;
            return RedirectToAction(nameof(PaymentDetails), new { paymentId });
        }

        string folderPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot", "uploads", "PaymentDocuments");

        Directory.CreateDirectory(folderPath);

        string fileName = $"{Guid.NewGuid()}{ext}";

        using (var stream = new FileStream(
            Path.Combine(folderPath, fileName), FileMode.Create))
        {
            await file!.CopyToAsync(stream);
        }

        await _paymentService.UploadDocument(new PaymentDocument
        {
            PaymentId = paymentId,
            DocumentType = documentType,
            FileName = fileName,
            OriginalFileName = Path.GetFileName(file.FileName),
            FilePath = "/uploads/PaymentDocuments/" + fileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            UploadedBy = GetUserId()
        });

        TempData["Success"] = "Document uploaded.";
        return RedirectToAction(nameof(PaymentDetails), new { paymentId });
    }

    //-------------------------------------------------------
    // Helpers
    //-------------------------------------------------------

    private int GetUserId()
    {
        var val = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(val, out int id))
            throw new UnauthorizedAccessException("User ID claim not found.");
        return id;
    }

    // Claim missing/0 ho to -1 return, taaki koi payment match na ho
    private int GetClaimId(string claimType)
    {
        return int.TryParse(User.FindFirst(claimType)?.Value, out int id) && id > 0
            ? id
            : -1;
    }

    // Admin sab dekh sakta hai; Hospital/Provider sirf apni payment
    private bool CanAccess(PaymentMaster payment)
    {
        if (User.IsInRole("Admin"))
            return true;

        if (User.IsInRole("Hospital"))
            return User.FindFirst("HospitalId")?.Value == payment.HospitalId.ToString();

        if (User.IsInRole("Provider"))
            return User.FindFirst("ProviderId")?.Value == payment.ProviderId.ToString();

        return false;
    }

    [HttpGet]
    [Authorize(Roles = "Hospital")]
    public async Task<IActionResult> GetAgreementDetails(int agreementId)
    {
        var result =
            await _paymentService.GetAgreementDetails(agreementId);

        if (result == null)
            return NotFound();

        if (result.HospitalId != GetClaimId("HospitalId"))
            return Forbid();

        return Json(result);
    }
    [HttpGet]
    [Authorize(Roles = "Hospital")]
    public async Task<IActionResult> GetAgreementsByProvider(int providerId)
    {
        int hospitalId = Convert.ToInt32(
            User.Claims.First(x => x.Type == "HospitalId").Value);

        var agreements = await _paymentService
            .GetAgreementsByProvider(
                hospitalId,
                providerId);

        return Json(agreements);
    }
    [HttpGet]
    [Authorize(Roles = "Hospital")]
    public async Task<IActionResult> GetPaymentCalculation(
    int agreementId,
    int hospitalId,
    int monthNo,
    int yearNo,
    int bedOccupancy)
    {
        hospitalId = GetClaimId("HospitalId");

        var result = await _paymentService.GetPaymentCalculation(
            agreementId,
            hospitalId,
            monthNo,
            yearNo,
            bedOccupancy);

        return Json(result);
    }
}