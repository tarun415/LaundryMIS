using LaudaryMis.Models;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
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
        var model = await _paymentService.GetPayments(
            agreementId,
            hospitalId,
            monthNo,
            yearNo,
            status);

        return View(model);
    }

    //-------------------------------------------------------
    // Generate Payment
    //-------------------------------------------------------

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
    public async Task<IActionResult> GeneratePayment(
     GeneratePaymentVM model)
    {
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

    public async Task<IActionResult> PaymentDetails(int paymentId)
    {
        var vm = new PaymentDetailsVM
        {
            Payment = await _paymentService.GetPaymentById(paymentId),
            Calculations = await _paymentService.GetCalculations(paymentId),
            Documents = await _paymentService.GetDocuments(paymentId),
            History = await _paymentService.GetApprovalHistory(paymentId)
        };

        if (vm.Payment == null)
            return NotFound();

        return View(vm);
    }

    //-------------------------------------------------------
    // Approve
    //-------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> ApprovePayment(
     int paymentId,
     string remarks)
    {
        bool result = await _paymentService.ApprovePayment(
            paymentId,
            1,
            remarks);

        TempData["Success"] = result
            ? "Payment Approved Successfully."
            : "Approval Failed.";

        return RedirectToAction(nameof(PaymentDetails),
            new { paymentId });
    }

    //-------------------------------------------------------
    // Reject
    //-------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> RejectPayment( int paymentId, string remarks)
    {
        bool result = await _paymentService.RejectPayment(
            paymentId,  1,  remarks);

        TempData["Success"] = result
            ? "Payment Rejected Successfully."
            : "Reject Failed.";

        return RedirectToAction(nameof(PaymentDetails),
            new { paymentId });
    }

    //-------------------------------------------------------
    // Upload Document
    //-------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> UploadPaymentDocument(
        PaymentDocument model)
    {
        await _paymentService.UploadDocument(model);
        return RedirectToAction(nameof(PaymentDetails), new { paymentId = model.PaymentId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAgreementDetails(int agreementId)
    {
        var result =
            await _paymentService.GetAgreementDetails(agreementId);

        if (result == null)
            return NotFound();

        return Json(result);
    }
    [HttpGet]
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
    public async Task<IActionResult> GetPaymentCalculation(
    int agreementId,
    int hospitalId,
    int monthNo,
    int yearNo,
    int bedOccupancy)
    {
        var result = await _paymentService.GetPaymentCalculation(
            agreementId,
            hospitalId,
            monthNo,
            yearNo,
            bedOccupancy);

        return Json(result);
    }
}