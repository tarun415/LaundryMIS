using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class HospitalController : Controller
    {
        private readonly IDailyService _service;
        private readonly IWPRService _wprService;
        private readonly IWebHostEnvironment _env;
        private readonly IDeliveryService _delservice;


        public HospitalController(IDailyService service, IWPRService wprService, IWebHostEnvironment env, IDeliveryService delservice)
        {
            _service = service;
            _wprService = wprService;
            _env = env;
            _delservice = delservice;
        }

        [Authorize(Roles = "Hospital")]

        public IActionResult Dashboard()
        {
            return View();
        }
        private int GetHospitalId()
        {
            return int.Parse(User.FindFirst("HospitalId").Value);
        }

        // 📊 LIST
        public async Task<IActionResult> VerificationList()
        {
            var hospitalId = GetHospitalId();

            var data = await _service.GetAllEntries();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyDelivery(int DeliveryId)
        {
            var model =
                await _delservice.GetDeliveryByIdAsync(DeliveryId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyDelivery(
            VerifyDeliveryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string uploadPath = Path.Combine(
                _env.WebRootPath,
                "Uploads",
                "LogBooks");

            int userId =
                Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            bool status =
                await _delservice.VerifyDeliveryAsync(
                    model,
                    userId,
                    uploadPath);

            if (!status)
            {
                ModelState.AddModelError(
                    "",
                    "Log book upload is mandatory or invalid file");

                return View(model);
            }

            TempData["Success"] =
                "Delivery verified successfully";

            return RedirectToAction("PendingVerification");
        }

    }
}
