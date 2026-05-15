using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<IActionResult>
       MonthlyVerification(
           int month = 0,
           int year = 0)
        {
            if (month == 0)
                month = DateTime.Now.Month;

            if (year == 0)
                year = DateTime.Now.Year;

            var hospitalId = GetHospitalId();

            ViewBag.Month = month;
            ViewBag.Year = year;

            var data =
                await _delservice
                    .GetWeeklyVerificationAsync(
                        hospitalId,
                        month,
                        year);

            // CHECK ALL VERIFIED

            ViewBag.IsMonthVerified =
                data.Count > 0 &&
                data.All(x => x.Status == "Verified");

            // CHECK LOGBOOK PATH

            var logBookPath =
                data
                .FirstOrDefault(x =>
                    !string.IsNullOrEmpty(x.LogBookPath))
                ?.LogBookPath;

            ViewBag.LogBookPath = logBookPath;

            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult>
    GetWeeklyDrillDown(
        int weekNo,
        int month,
        int year)
        {
            var hospitalId = GetHospitalId();

            var data =
                await _delservice
                    .GetWeeklyDrillDownAsync(
                        hospitalId,
                        month,
                        year,
                        weekNo);

            return Json(data);
        }

       

        [HttpPost]
        public async Task<IActionResult> SaveWeeklyVerificationLog(
     WeeklyVerificationModel model)
        {
            try
            {
                int result =
                    await _delservice
                    .SaveWeeklyVerificationLogAsync(model);

                return Json(new
                {
                    success = true,
                    message = "Row verified successfully."
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



        [HttpPost]
        public async Task<IActionResult> SaveMonthlyLogBook(
      WeeklyVerificationModel model)
        {
            try
            {
                string fileName = "";

                if (model.LogBookFile != null)
                {
                    string folderPath =
                        Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/Uploads/WeeklyLogBook");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    fileName =
                        Guid.NewGuid().ToString()
                        + Path.GetExtension(model.LogBookFile.FileName);

                    string filePath =
                        Path.Combine(folderPath, fileName);

                    using (var stream =
                        new FileStream(filePath, FileMode.Create))
                    {
                        await model.LogBookFile.CopyToAsync(stream);
                    }

                    model.LogBookPath =
                        "/Uploads/WeeklyLogBook/" + fileName;
                }

                model.HospitalId = GetHospitalId();

                int result =
                    await _delservice
                        .SaveMonthlyLogBookAsync(model);

                TempData["Success"] =
                    "Monthly LogBook saved successfully.";

                return RedirectToAction(
                    "MonthlyVerification",
                    new
                    {
                        month = model.Month,
                        year = model.Year
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("MonthlyVerification");
            }
        }
    }
}
