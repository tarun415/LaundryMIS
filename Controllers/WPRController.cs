using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class WPRController : Controller
    {
        private readonly IWPRService _wprService;
        private readonly IAgreementRepository _agreementRepository;  // ← ADD THIS


        public WPRController(IWPRService wprService, IAgreementRepository agreementRepository)
        {
            _wprService = wprService;
            _agreementRepository = agreementRepository;

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

        [HttpGet("api/agreement/{agreementId}")]
        public async Task<IActionResult> GetAgreementDetails(int agreementId)
        {
            try
            {
                var agreement = await _agreementRepository.GetByIdAsync(agreementId);

                if (agreement == null)
                    return NotFound(new { message = "Agreement not found" });

                return Ok(new
                {
                    id = agreement.Id,
                    providerId = agreement.ProviderId,
                    hospitalId = agreement.HospitalId,
                    hospitalName = agreement.HospitalName  // ← Include Hospital Name

                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}