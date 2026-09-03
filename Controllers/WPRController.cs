using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize]
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
        //[HttpGet]
        //public IActionResult WPREntry(int agreementId = 0)
        //{
        //    ViewBag.AgreementId = agreementId;
        //    return View(new WPRVM { AgreementId = agreementId });
        //}

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
                ViewBag.HospitalId = agreement.HospitalId;
                ViewBag.ProviderId = agreement.ProviderId;
                return Ok(new
                {
                    id = agreement.Id,
                    providerId = agreement.ProviderId,
                    hospitalId = agreement.HospitalId,
                    hospitalName = agreement.HospitalName,  // ← Include Hospital Name
                     providerName = agreement.ProviderName  // ← Include Provider Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckWeeklyVerification(
     int weekNo,
     int month,
     int year)
        {
            bool isVerified =
                await _wprService.CheckWeeklyVerification(
                    weekNo,
                    month,
                    year);

            return Json(new
            {
                success = isVerified
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyPerformanceData(
      int agreementId,
      int hospitalId,
      int weekNo,
      int month,
      int year)
        {
            try
            {
                var data =
                    await _wprService.GetWeeklyPerformanceData(
                        agreementId,
                        hospitalId,
                        weekNo,
                        month,
                        year);

                return Json(new
                {
                    success = true,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        private int GetHospitalId()
        {
            var claim = User.FindFirst("HospitalId")?.Value;
            if (!int.TryParse(claim, out int id) || id <= 0)
                throw new UnauthorizedAccessException();
            return id;
        }
        [HttpGet]
        public async Task<IActionResult> WPREntry()
        {
            int hospitalId = GetHospitalId();

            var agreement = await _agreementRepository.GetByHosIdAsync(hospitalId);

            if (agreement == null)
            {
                TempData["Error"] = "No active agreement found for this hospital.";
               return View(new WPRVM());
            }

            ViewBag.AgreementId = agreement.Id;
            ViewBag.ProviderName = agreement.ProviderName;
            ViewBag.HospitalId = agreement.HospitalId;
            ViewBag.ProviderId = agreement.ProviderId;


            return View(new WPRVM
            {
                AgreementId = agreement.Id,
                HospitalId = hospitalId,
                ProviderId = agreement.ProviderId,
                StaffName = agreement.ProviderName
            });
        }
    }
}