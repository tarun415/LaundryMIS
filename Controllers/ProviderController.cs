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
        private readonly IAccessGuard _guard;
        public ProviderController(
            IDailyService service,
            IProviderService providerService,
            IWPRService wprService, IHospitalService hosservice, IWardService wardservice, IPickUpService pkservice, IDeliveryChallanService delservice,
            IAccessGuard guard)
        {
            _guard = guard;
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

        private async Task<bool> OwnsEntry(int entryId) =>
            await _service.IsEntryOwnedByProviderAsync(entryId, GetProviderId());

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] DailyEntryVM model)
        {
            var providerId = GetProviderId();

            var allowedHospitals = await _service.GetHospitalsByProvider(providerId);

            if (!allowedHospitals.Any(h => h.HospitalId == model.HospitalId))
                return BadRequest("Invalid hospital selection");

            // Editing an existing entry: it must belong to this provider.
            if (model.EntryId > 0 && !await OwnsEntry(model.EntryId))
                return Forbid();

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
            if (!await OwnsEntry(id)) return Forbid();
            await _service.UpdateStatus(id, "Delivered");
            return Ok();
        }

        public async Task<IActionResult> WPREntry()
        {
            return View();
        }

        public async Task<IActionResult> DailyEntryList()
        {
            var data = await _service.GetAllEntries(providerId: GetProviderId());
            return View(data);
        }
        public async Task<IActionResult> DailyEntryItems(int id)
        {
            var data = await _service.GetAllItems(id, GetProviderId());
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
            if (!await OwnsEntry(id)) return Forbid();
            var vm = await _service.GetEntryForDelivery(id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deliver([FromBody] DeliveryVM model)
        {
            if (!await OwnsEntry(model.EntryId)) return Forbid();
            var id = await _service.DeliverAsync(model);
            return Ok(new { success = true, id });
        }

        [HttpGet]
        public async Task<JsonResult> GetProviders()
        {
            // A vendor only sees itself, not the other vendors
            var providerId = GetProviderId();
            var data = (await _ProviderService.GetAll()).Where(p => p.ProviderId == providerId);
            return Json(data);
        }

        // 🔥 WPR GET
        // Filter dropdown: only the hospitals this vendor has an active agreement with
        public async Task<IActionResult> GetHospitals()
        {
            var data = await _service.GetHospitalsByProvider(GetProviderId());
            return Json(data.Select(h => new { id = h.HospitalId, name = h.HospitalName }));
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
            var data = await _service.SearchDailyEntries(status, hospitalId, wardId, date, GetProviderId());
            return Json(data);
        }
        // EDIT
        public async Task<IActionResult> EditDailyEntry(int id)
        {
            if (!await OwnsEntry(id)) return Forbid();
            var data = await _service.GetDailyEntryByIdAsync(id);
            return View("DailyEntry", data);
        }

        // DELETE
        [HttpPost]
        public async Task<IActionResult> DeleteDailyEntry(int id)
        {
            try
            {
                if (!await OwnsEntry(id))
                    return Json(new { success = false, message = "Not allowed" });

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
            var providerId = GetProviderId();

            var data = (await _pkservice.GetPickupList())
                .Where(x => x.ProviderId == providerId)
                .ToList();
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
                if (!await _guard.CanAccessPickupAsync(User, model.PickupId))
                    return Json(new { success = false, message = "You are not allowed to accept this pickup." });

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
            if (!await _guard.CanAccessPickupAsync(User, id)) return Forbid();

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
                if (!await _guard.CanAccessPickupAsync(User, model.PickupId))
                    return Json(new { success = false, message = "You are not allowed to deliver this pickup." });

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
            var visible = await _guard.VisibleDeliveryIdsAsync(User);

            var model = (await _delservice.GetDeliveryList())
                .Where(x => visible == null || visible.Contains(x.DeliveryId))
                .ToList();

            return View(model);
        }
        public async Task<IActionResult>
DeliveryItems(int id)
        {
            if (!await _guard.CanAccessDeliveryAsync(User, id)) return Forbid();

            var data =
                await _delservice.GetDeliveryItems(id);

            return Json(data);
        }
        #endregion

    }
}