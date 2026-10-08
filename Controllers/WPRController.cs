using LaudaryMis.Helpers;
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
        private readonly IAccessGuard _guard;

        public WPRController(IWPRService wprService, IAgreementRepository agreementRepository, IAccessGuard guard)
        {
            _guard = guard;
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
        [Authorize(Roles = "Hospital")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WPREntry(WPRVM model)
        {
            // The hospital comes from the login and the agreement must be its own; the vendor is
            // taken from that agreement, so none of the posted ids can point at another hospital.
            model.HospitalId = GetHospitalId();

            var agreement = await _guard.GetAgreementOwnerAsync(model.AgreementId);
            if (agreement == null || agreement.HospitalId != model.HospitalId)
                return Forbid();

            model.ProviderId = agreement.ProviderId;

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

        // The hospital's own submitted WPRs and what the CMS has done with them (read only:
        // a submitted WPR cannot be changed by the hospital)
        [HttpGet]
        [Authorize(Roles = "Hospital")]
        public async Task<IActionResult> MyWprs(string? status, int? month, int? year)
        {
            if (status != WprStatus.Pending && status != WprStatus.Verified) status = null;

            ViewBag.Status = status;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(await _wprService.GetListAsync(GetHospitalId(), null, status, month, year));
        }

        [HttpGet("api/agreement/{agreementId}")]
        public async Task<IActionResult> GetAgreementDetails(int agreementId)
        {
            try
            {
                var agreement = await _agreementRepository.GetByIdAsync(agreementId);

                if (agreement == null)
                    return NotFound(new { message = "Agreement not found" });

                // Only the hospital / vendor party to the agreement (or admin) may read it
                if (!User.CanSee(agreement.HospitalId, agreement.ProviderId))
                    return Forbid();

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
        [Authorize(Roles = "Hospital")]
        public async Task<JsonResult> CheckWeeklyVerification(
     int weekNo,
     int month,
     int year)
        {
            bool isVerified =
                await _wprService.CheckWeeklyVerification(
                    GetHospitalId(),
                    weekNo,
                    month,
                    year);

            return Json(new
            {
                success = isVerified
            });
        }

        [HttpGet]
        [Authorize(Roles = "Hospital")]
        public async Task<IActionResult> GetWeeklyPerformanceData(
      int agreementId,
      int hospitalId,
      int weekNo,
      int month,
      int year)
        {
            try
            {
                // Always this hospital's own data, whatever hospitalId the query string carries
                hospitalId = GetHospitalId();

                if (!await _guard.CanAccessAgreementAsync(User, agreementId))
                    return Json(new { success = false, message = "Agreement not found." });

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