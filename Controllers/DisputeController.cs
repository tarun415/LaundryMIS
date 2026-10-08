using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    // Bill disputes. The service provider raises them; the CMS of the hospital and the admin see them;
    // only the admin can correct the WPR (with the CMS letter) or reject.
    [Authorize]
    public class DisputeController : Controller
    {
        private readonly IDisputeService _disputes;
        private readonly IBillService _bills;

        public DisputeController(IDisputeService disputes, IBillService bills)
        {
            _disputes = disputes;
            _bills = bills;
        }

        // ── Service provider ──────────────────────────────────

        [HttpGet]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> Raise()
        {
            var vm = new RaiseDisputeVM
            {
                Year = DateTime.Today.Year,
                Hospitals = await _bills.GetHospitalsAsync(GetProviderId())
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "ServiceProvider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Raise(RaiseDisputeVM model)
        {
            var providerId = GetProviderId();
            model.Hospitals = await _bills.GetHospitalsAsync(providerId);

            if (!ModelState.IsValid)
                return View(model);

            var (ok, message) = await _disputes.RaiseAsync(providerId, GetUserId(), model);
            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Mine));
        }

        // The months of a hospital + year whose bill was printed (for the month dropdown)
        [HttpGet]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> PrintedMonths(int hospitalId, int year) =>
            Json(await _bills.GetDisputableMonthsAsync(GetProviderId(), hospitalId, year));

        [HttpGet]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> Mine() =>
            View(await _disputes.GetListAsync(GetProviderId(), null, null));

        // ── Admin ─────────────────────────────────────────────

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? status)
        {
            ViewBag.Status = status;
            return View(await _disputes.GetListAsync(null, null, string.IsNullOrWhiteSpace(status) ? null : status));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Resolve(int id)
        {
            var vm = await _disputes.GetResolveViewAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(12 * 1024 * 1024)]
        public async Task<IActionResult> Resolve(DisputeResolvePost post)
        {
            var (ok, message) = await _disputes.ResolveAsync(GetUserId(), post);

            TempData[ok ? "Success" : "Error"] = message;
            return ok
                ? RedirectToAction(nameof(Index))
                : RedirectToAction(nameof(Resolve), new { id = post.DisputeId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? remarks)
        {
            var (ok, message) = await _disputes.RejectAsync(GetUserId(), id, remarks);

            TempData[ok ? "Success" : "Error"] = message;
            return ok
                ? RedirectToAction(nameof(Index))
                : RedirectToAction(nameof(Resolve), new { id });
        }

        // ── The CMS letter ────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Letter(int id)
        {
            var dispute = await _disputes.GetAsync(id);
            if (dispute == null) return NotFound();

            // Admin sees every letter, a CMS / hospital those of its own hospital, a vendor those of its own disputes
            bool allowed = User.IsAdmin()
                || (User.IsInRole("CMS") && User.HospitalId() == dispute.HospitalId)
                || (User.IsInRole("ServiceProvider") && User.ProviderId() == dispute.ProviderId);
            if (!allowed) return Forbid();

            var path = _disputes.LetterPath(dispute);
            if (path == null) return NotFound();

            var ext = Path.GetExtension(path).ToLowerInvariant();
            var contentType = ext switch
            {
                ".pdf" => "application/pdf",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return PhysicalFile(path, contentType, dispute.LetterOriginalName ?? Path.GetFileName(path));
        }

        private int GetProviderId() =>
            User.ProviderId() ?? throw new UnauthorizedAccessException();

        private int GetUserId() =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id
                : throw new UnauthorizedAccessException();
    }
}
