using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    // The service provider's side: WPR status per hospital and the printable monthly bill.
    // The bill is built from the CMS-verified WPRs, so there is nothing to draft or submit.
    [Authorize(Roles = "ServiceProvider")]
    public class BillController : Controller
    {
        private readonly IBillService _bills;
        private readonly IWPRService _wpr;

        public BillController(IBillService bills, IWPRService wpr)
        {
            _bills = bills;
            _wpr = wpr;
        }

        // WPRs of the vendor's hospitals with their status (Pending / Verified)
        [HttpGet]
        public async Task<IActionResult> WprStatus(string? status, int? hospitalId, int? month, int? year)
        {
            if (status != ViewModels.WprStatus.Pending && status != ViewModels.WprStatus.Verified) status = null;

            ViewBag.Status = status;
            ViewBag.HospitalId = hospitalId;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Hospitals = await _bills.GetHospitalsAsync(GetProviderId());

            var list = await _wpr.GetListAsync(hospitalId, GetProviderId(), status, month, year);
            return View(list);
        }

        // Months with their bill state, and the print history
        [HttpGet]
        public async Task<IActionResult> MyBills()
        {
            var providerId = GetProviderId();
            ViewBag.PrintLog = await _bills.GetPrintLogAsync(providerId);
            return View(await _bills.GetMonthsAsync(providerId));
        }

        // The printable bill. Opening it counts as a print, so the month becomes disputable.
        [HttpGet]
        public async Task<IActionResult> Print(int hospitalId, int month, int year)
        {
            var providerId = GetProviderId();

            var (bill, message) = await _bills.GetBillAsync(providerId, hospitalId, month, year);
            if (bill == null)
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(MyBills));
            }

            await _bills.LogPrintAsync(bill, GetUserId());
            bill.PrintedAt = DateTime.Now;
            return View(bill);
        }

        private int GetProviderId() =>
            User.ProviderId() ?? throw new UnauthorizedAccessException();

        private int GetUserId() =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id
                : throw new UnauthorizedAccessException();
    }
}
