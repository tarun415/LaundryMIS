using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Provider")]
    public class ProviderController : Controller
    {
        private readonly IDailyService _service;
        private readonly IProviderService _ProviderService;
        private readonly IWPRService _wprService;
        private readonly IHospitalService _hosservice;
        private readonly IWardService _wardservice;
        private readonly IPickUpService _pkservice;
        private readonly IDeliveryChallanService _delservice;
        public ProviderController(
            IDailyService service,
            IProviderService providerService,
            IWPRService wprService, IHospitalService hosservice, IWardService wardservice, IPickUpService pkservice, IDeliveryChallanService delservice)
        {
            _service = service;
            _hosservice = hosservice;
            _ProviderService = providerService;
            _wprService = wprService;
            _wardservice = wardservice;
            _pkservice = pkservice;
            _delservice = delservice;
        }

        private int GetProviderId()
        {
            var claim = User.FindFirst("ProviderId")?.Value;
            if (!int.TryParse(claim, out int id) || id <= 0)
                throw new UnauthorizedAccessException();

            return id;
        }

        //public IActionResult Dashboard() => View();
        public IActionResult Dashboard()
        {
            return View();
        }

        //✅ GET
        public async Task<IActionResult> DailyEntry()
        {
            var providerId = GetProviderId();

            var vm = new DailyEntryVM();

            vm.EntryDate = DateTime.Now;

            vm.Hospitals = await _service.GetHospitalsByProvider(providerId);
            vm.Wards = await _service.GetWards();

            // 🔥 ADD THIS LINE (MISSING)
            vm.LinenTypes = await _service.GetLinenTypes();

            return View(vm);
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Save([FromBody] DailyEntryVM model)
        {
            var providerId = GetProviderId();

            var allowedHospitals = await _service.GetHospitalsByProvider(providerId);

            if (!allowedHospitals.Any(h => h.HospitalId == model.HospitalId))
                return BadRequest("Invalid hospital selection");

            model.ProviderId = providerId;

            // 🔥 NEW
            model.Status = "Collected";

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Items == null || !model.Items.Any(x => x.DirtyCount > 0))
                return BadRequest("No dirty linen entered");

            var id = await _service.SaveAsync(model);

            return Ok(new { success = true, id });
        }

        [HttpPost]
        public async Task<IActionResult> MarkDelivered(int id)
        {
            await _service.UpdateStatus(id, "Delivered");
            return Ok();
        }

        public async Task<IActionResult> WPREntry()
        {
            return View();
        }

        public async Task<IActionResult> DailyEntryList()
        {
            var data = await _service.GetAllEntries();
            return View(data);
        }
        public async Task<IActionResult> DailyEntryItems(int id)
        {
            var data = await _service.GetAllItems(id);
            return Json(data);
        }

        public async Task<IActionResult> Pending()
        {
            var providerId = GetProviderId();

            var data = await _service.GetPendingEntries(providerId);

            return View(data);
        }

        public async Task<IActionResult> Deliver(int id)
        {
            var vm = await _service.GetEntryForDelivery(id);
            return View(vm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Deliver([FromBody] DeliveryVM model)
        {
            var id = await _service.DeliverAsync(model);
            return Ok(new { success = true, id });
        }

        [HttpGet]
        public async Task<JsonResult> GetProviders()
        {
            var data = await _ProviderService.GetAll();
            return Json(data);
        }

        // 🔥 WPR GET
        public async Task<IActionResult> GetHospitals()
        {
            var data = await _hosservice.GetHospitalNamesAsync();
            return Json(data);
        }
        public async Task<IActionResult> GetWards()
        {
            var data = await _wardservice.GetWardNamesAsync();
            return Json(data);
        }
        //Search
        [HttpGet]
        public async Task<IActionResult> SearchDailyEntries(string status, int? hospitalId, int? wardId, DateTime? date)
        {
            var data = await _service.SearchDailyEntries(status, hospitalId, wardId, date);
            return Json(data);
        }
        // EDIT
        public async Task<IActionResult> EditDailyEntry(int id)
        {
            var data = await _service.GetDailyEntryByIdAsync(id);
            return View("DailyEntry", data);
        }

        // DELETE
        [HttpPost]
        public async Task<JsonResult> DeleteDailyEntry(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Entry not found" });

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #region New Development 
        public async Task<IActionResult> AcceptPickup(int id)
        {
           
            var data = await _pkservice.GetPickupList();
            return View(data);
            // var providerId = GetProviderId();
            //var model =
            //    await _pkservice
            //    .GetPickupForAcceptance(providerId);

            //if (model == null)
            //{
            //    return RedirectToAction("PickupList");
            //}

            //return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptPickup(PickupVM model)
        {
            try
            {
                int userId = Convert.ToInt32(
     User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _pkservice.AcceptPickup(
                    model.PickupId,
                    userId,
                    model.Remarks);

                return Json(new
                {
                    success = true,
                    message = "Pickup accepted successfully."
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
        public async Task<IActionResult> DeliveryChallan(int id)
        {
            var model =
                await _delservice.GetPickupForDelivery(id);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeliveryChallan(
    [FromBody] DeliveryChallanVM model)
        {
            try
            {
                var challanId =
                    await _delservice.SaveDelivery(model);

                return Json(new
                {
                    success = true,
                    challanId
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

        public async Task<IActionResult> DeliveryList()
        {
            var model =
                await _delservice.GetDeliveryList();

            return View(model);
        }
        public async Task<IActionResult>
DeliveryItems(int id)
        {
            var data =
                await _delservice.GetDeliveryItems(id);

            return Json(data);
        }
        #endregion

    }
}