using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System.Data;
using System.Drawing;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    public class HospitalController : Controller
    {
        private readonly IDailyService _service;
        private readonly IWPRService _wprService;
        private readonly IWebHostEnvironment _env;
        private readonly IDeliveryService _delservice;
        private readonly IPickUpService _pkservice;
        private readonly IHospitalService _hosservice;
        private readonly IWardService _wardservice;
        private readonly IProviderService _ProviderService;
        private readonly IDailyService _dservice;
        private readonly ICommonService _comservice;
        private readonly IDeliveryChallanService _deliverychanService;

        public HospitalController(IDailyService service, IWPRService wprService, IWebHostEnvironment env, IDeliveryService delservice,IHospitalService hosservice ,IWardService wardservice, IProviderService ProviderService, IDailyService dailyService, IPickUpService pkservice, ICommonService comservice, IDeliveryChallanService deliverychanService)
                {
                    _service = service;
                    _wprService = wprService;
                    _env = env;
                    _delservice = delservice;
            _hosservice = hosservice;
            _wardservice = wardservice;
            _ProviderService = ProviderService;
            _dservice = dailyService;
            _pkservice = pkservice;
            _comservice = comservice;
            _deliverychanService = deliverychanService;
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


        #region New LMS 


        // CREATE PAGE

        [HttpGet]
        public async Task<IActionResult> CreatePickUp()
        {
            var model = new PickupVM();

            // dropdown data

            var hospitalId = GetHospitalId();
            model.Wards = await _comservice.GetWards();

            model.Providers = await _comservice.GetProviderByHospital(hospitalId);

            model.LinenTypes = await _comservice.GetLinenTypes();

            model.Items = new List<PickupItemVM>();

            model.HospitalId = hospitalId;
            var agreement = await _comservice.GetAgreementByHospital(model.HospitalId);

            if (agreement != null)
            {
                model.AgreementId = agreement.AgreementId;
                model.ProviderId = agreement.ProviderId;
            }

            return View(model);
        }
        // SAVE PICKUP
        [HttpPost]
        public async Task<JsonResult> SavePickup( [FromBody] PickupVM model)
        {
            try
            {
                model.CreatedBy =
                    Convert.ToInt32(
                        User.FindFirst("UserId")?.Value);

                var pickupId =
                    await _pkservice.SavePickup(model);

                return Json(new
                {
                    success = true,
                    pickupId = pickupId,
                    message = "Pickup saved successfully"
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
        // LIST
        public async Task<IActionResult> PickupList()
        {
            var data = await _pkservice.GetPickupList();
            return View(data);
        }

        // CHILD ITEMS
        [HttpGet]
        public async Task<IActionResult> PickupItems(int id)
        {
            var data = await _pkservice.GetPickupItems(id);
            return Json(data);
        }
        // DELETE
        [HttpPost]
        public async Task<IActionResult> DeletePickup(int id)
        {
            var result = await _pkservice.DeletePickup(id);

            return Json(new
            {
                success = result.Flag == 1,
                message = result.Message
            });
        }
        // SEARCH
        [HttpGet]
        public async Task<IActionResult> SearchPickupList(
            string status,
            int? hospitalId,
            int? wardId,
            DateTime? date)
        {
            var data = await _pkservice.SearchPickupList(
                status,
                hospitalId,
                wardId,
                date);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> EditPickup(int id)
        {
            var model = await _pkservice.GetPickupById(id);

            if (model == null)
                return NotFound();

            // Dropdowns
            model.Wards =
                await _comservice.GetWards();

            model.Providers =
                await _comservice.GetProviderByHospital(
                    model.HospitalId);

            model.LinenTypes =
                await _comservice.GetLinenTypes();

            return View("CreatePickUp", model);
        }

        [HttpGet]
        public async Task<IActionResult> PrintPickup(int id)
        {
            var model = await _pkservice.GetPickupById(id);

            if (model == null)
                return NotFound();

            // Create folder if not exists
            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Uploads",
                "Pickups");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // File name
            string fileName =
                $"Pickup_{id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            string fullPath =
                Path.Combine(folderPath, fileName);

            // Generate PDF
            var pdf = new ViewAsPdf("PrintPickup", model)
            {
                FileName = fileName,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Orientation.Portrait
            };

            byte[] pdfBytes =
                await pdf.BuildFile(ControllerContext);

            // Save PDF physically
            await System.IO.File.WriteAllBytesAsync(
                fullPath,
                pdfBytes);

            // Save URL in DB
            string dbPath =
                "/Uploads/Pickups/" + fileName;

            await _pkservice.UpdatePrintUrl(
                id,
                dbPath);

            // Return PDF to browser
            return File(
                pdfBytes,
                "application/pdf",
                fileName);
        }
        
        // [HttpPost]
        //    public async Task<IActionResult> AcceptDelivery(
        //int PickupId,
        //string remarks)
        //    {
        //        try
        //        {
        //            int userId =
        //                Convert.ToInt32(
        //                    User.FindFirst(
        //                        ClaimTypes.NameIdentifier)?.Value);

        //            await _pkservice.AcceptDelivery(
        //                PickupId,
        //                userId,
        //                remarks);

        //            return Json(new
        //            {
        //                success = true,
        //                message = "Delivery accepted successfully."
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new
        //            {
        //                success = false,
        //                message = ex.Message
        //            });
        //        }
        //    }

        [HttpPost]
        public async Task<IActionResult> AcceptDelivery(
    int DeliveryId,
    string remarks)
        {
            try
            {
                int userId =
                    Convert.ToInt32(
                        User.FindFirst(
                            ClaimTypes.NameIdentifier)?.Value);

                await _pkservice.AcceptDelivery(
                    DeliveryId,
                    userId,
                    remarks);

                return Json(new
                {
                    success = true,
                    message = "Delivery accepted successfully."
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
        [HttpGet]
        public async Task<IActionResult>GetPickupDeliveryHistory(int id)
        {
            var result =await _pkservice.GetPickupDeliveryHistory(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult>
VerifyDeliveries(
    int PickupId,
    string DeliveryIds,
    string Remarks)
        {
            try
            {
                int userId =
                    Convert.ToInt32(
                        User.FindFirst(
                            ClaimTypes.NameIdentifier)?.Value);

                await _pkservice.VerifyDeliveries(
                    PickupId,
                    DeliveryIds,
                    userId,
                    Remarks);

                return Json(new
                {
                    success = true,
                    message =
                    "Deliveries verified successfully."
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
        [HttpGet]
        public async Task<JsonResult>
GetDeliveryItems(int deliveryId)
        {
            var result =
                await _deliverychanService
                .GetDeliveryItems(deliveryId);

            return Json(result);
        }
        #endregion

    }
}
