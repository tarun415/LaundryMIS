using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userservice;
        private readonly IHospitalService _hospitalService;
        private readonly IProviderService _providerService;

        public UserController(IUserService userservice, IHospitalService hospitalService, IProviderService providerService)
        {
            _userservice = userservice;
            _hospitalService = hospitalService;
            _providerService = providerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDistricts()
        {
            var data = await _hospitalService.GetDistricts(); 
            return Json(data);
        }


        [HttpGet]
        public async Task<JsonResult> GetHospitalsByDistrict(int districtId)
        {
            var data = await _hospitalService.GetHospitalsByDistrict(districtId);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetProviders()
        {
            var data = await _providerService.GetProviderAsync();
            return Json(data);
        }
    }
}
