using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    // The hospital's CMS: reviews, edits and verifies the WPRs the hospital submits
    [Authorize(Roles = "CMS")]
    public class CMSController : Controller
    {
        private readonly IWPRService _wpr;
        private readonly IDisputeService _disputes;

        public CMSController(IWPRService wpr, IDisputeService disputes)
        {
            _wpr = wpr;
            _disputes = disputes;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var hospitalId = GetHospitalId();

            var vm = await _wpr.GetCmsDashboardAsync(hospitalId);
            vm.OpenDisputes = await _disputes.CountOpenAsync(hospitalId);

            var name = User.Identity?.Name ?? "";
            vm.HospitalName = name.StartsWith("CMS - ") ? name["CMS - ".Length..] : name;

            return View(vm);
        }

        // WPRs of this hospital, filtered by status / month / year
        [HttpGet]
        public async Task<IActionResult> WprList(string? status, int? month, int? year)
        {
            if (status != WprStatus.Pending && status != WprStatus.Verified) status = null;

            ViewBag.Status = status;
            ViewBag.Month = month;
            ViewBag.Year = year;

            var list = await _wpr.GetListAsync(GetHospitalId(), null, status, month, year);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var vm = await _wpr.GetReviewAsync(id);
            if (vm == null || vm.HospitalId != GetHospitalId()) return NotFound();
            return View(vm);
        }

        // Save the edited scores, and verify too when the "Verify & Accept" button was used
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(WprReviewPost post)
        {
            var (ok, message) = await _wpr.CmsSaveAsync(GetHospitalId(), GetUserId(), post);

            TempData[ok ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Review), new { id = post.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(int id, string? returnTo)
        {
            var (ok, message) = await _wpr.CmsVerifyAsync(GetHospitalId(), GetUserId(), id);

            TempData[ok ? "Success" : "Error"] = message;
            return returnTo == "review"
                ? RedirectToAction(nameof(Review), new { id })
                : RedirectToAction(nameof(WprList), new { status = WprStatus.Pending });
        }

        // Verify every pending WPR of a month at once
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyMonth(int month, int year)
        {
            var (ok, message) = await _wpr.CmsVerifyMonthAsync(GetHospitalId(), GetUserId(), month, year);

            TempData[ok ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(WprList), new { status = WprStatus.Pending });
        }

        // Disputes the vendor raised on this hospital's bills (read only)
        [HttpGet]
        public async Task<IActionResult> Disputes(string? status)
        {
            ViewBag.Status = status;
            var list = await _disputes.GetListAsync(null, GetHospitalId(), string.IsNullOrWhiteSpace(status) ? null : status);
            return View(list);
        }

        private int GetHospitalId() =>
            User.HospitalId() ?? throw new UnauthorizedAccessException();

        private int GetUserId() =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id
                : throw new UnauthorizedAccessException();
    }
}
