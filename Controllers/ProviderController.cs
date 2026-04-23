using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Provider")]
    public class ProviderController : Controller
    {
        private readonly IDailyService _service;

        public ProviderController(IDailyService service)
        {
            _service = service;
        }

        private int GetProviderId()
        {
            var claim = User.FindFirst("ProviderId")?.Value;

            if (!int.TryParse(claim, out int id) || id <= 0)
                throw new UnauthorizedAccessException();

            return id;
        }

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
    }
}