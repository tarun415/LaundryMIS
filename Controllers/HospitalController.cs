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

        public HospitalController(IDailyService service, IWPRService wprService)
        {
            _service = service;
            _wprService = wprService;
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


        public async Task<IActionResult> WPREntry()
        {
            var vm = new WPRVM();

            vm.Parameters = await _wprService.GetParameters();
            vm.Agreements = await _wprService.GetHospitalAgreements(GetHospitalId());

            // ✅ DEFAULT VALUES (important)
            vm.Month = DateTime.Now.Month;
            vm.Year = DateTime.Now.Year;

            // 👉 optional: default week (current week logic simple)
            vm.Week = 1;

            // ✅ IMPORTANT: initialize details (binding fix)
            vm.Details = vm.Parameters.Select(p => new WPRDetail
            {
                ParameterId = p.Id
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> WPREntry(WPRVM model)
        {
            try
            {
                var hospitalId = GetHospitalId();

                await _wprService.SaveAsync(model, hospitalId);

                TempData["msg"] = "WPR Saved Successfully";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction("WPREntry");
        }
    }
}
