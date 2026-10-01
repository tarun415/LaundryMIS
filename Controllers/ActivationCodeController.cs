using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    // Admin issues one-time activation codes to hospitals and vendors that are
    // already on the tender list, so they can register themselves.
    [Authorize(Roles = "Admin")]
    public class ActivationCodeController : Controller
    {
        private readonly IActivationCodeService _service;

        public ActivationCodeController(IActivationCodeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string type = "Hospital")
        {
            type = NormalizeType(type);
            return View(new ActivationIndexVM
            {
                EntityType = type,
                Rows = await _service.GetStatusAsync(type)
            });
        }

        // Issue codes for the chosen rows, or for everyone who has neither a login nor a code.
        // The codes are shown once; only their hashes are stored.
        [HttpPost]
        public async Task<IActionResult> Issue(string type, int[]? ids, bool allMissing = false)
        {
            type = NormalizeType(type);

            var targets = new List<int>();
            if (allMissing)
            {
                targets.AddRange((await _service.GetStatusAsync(type))
                    .Where(r => r.Status == "NoCode").Select(r => r.EntityId));
            }
            if (ids != null) targets.AddRange(ids);

            var codes = await _service.IssueAsync(type, targets.Distinct().ToList(), GetUserId());

            if (codes.Count == 0)
            {
                TempData["Error"] = "No hospital / firm found to issue codes for " +
                                    "(codes are not issued to those that already have an account).";
                return RedirectToAction(nameof(Index), new { type });
            }

            // Codes must not be cached or end up in the browser history
            Response.Headers["Cache-Control"] = "no-store";
            return View("Issued", new ActivationIssuedVM { EntityType = type, Codes = codes });
        }

        private static string NormalizeType(string? type) =>
            string.Equals(type, "Provider", StringComparison.OrdinalIgnoreCase) ? "Provider" : "Hospital";

        private int GetUserId() =>
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
    }
}
