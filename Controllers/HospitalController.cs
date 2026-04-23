using LaudaryMis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class HospitalController : Controller
    {
        private readonly IDailyService _service;

        public HospitalController(IDailyService service)
        {
            _service = service;
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

      
    }
}
