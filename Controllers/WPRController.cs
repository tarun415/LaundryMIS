using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class WPRController : Controller
    {
        private readonly IWPRService _wprService;

        public WPRController(IWPRService wprService)
        {
            _wprService = wprService;
        }

        // GET: /WPR/WPREntry?agreementId=1
        [HttpGet]
        public IActionResult WPREntry(int agreementId = 0)
        {
            ViewBag.AgreementId = agreementId;
            return View(new WPRVM { AgreementId = agreementId });
        }

        // POST: /WPR/WPREntry
        [HttpPost("WPR/WPREntry")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WPREntry(WPRVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, message) = await _wprService.SubmitWPRAsync(model);

            if (!success)
            {
                TempData["Error"] = message;
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(WPREntry));
        }
    }
}