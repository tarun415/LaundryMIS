using LaudaryMis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonService _comservice;

        public CommonController(ICommonService comservice)
        {
            _comservice = comservice;
        }

        // =========================
        // WARDS
        // =========================

        [HttpGet]
        public async Task<JsonResult> GetWards()
        {
            var data = await _comservice.GetWards();

            return Json(data.Select(x => new
            {
                id = x.Id,
                name = x.Name
            }));
        }

        // =========================
        // PROVIDERS BY HOSPITAL
        // =========================

        [HttpGet]
        public async Task<JsonResult> GetProviders(int hospitalId)
        {
            var data =
                await _comservice
                    .GetProviderByHospital(hospitalId);

            return Json(data);
        }

        // =========================
        // HOSPITALS BY PROVIDER
        // =========================

        [HttpGet]
        public async Task<JsonResult> GetHospitals(int providerId)
        {
            var data =
                await _comservice
                    .GetHospitalsByProvider(providerId);

            return Json(data);
        }
        //Delivery Report for both provider and Hospital
        public async Task<IActionResult>
DeliverySummaryReport()
        {
            var model =
                await _comservice
                    .GetDeliverySummaryReport();

            return View(model);
        }
        public async Task<IActionResult>
DeliveryHistory(int id)
        {
            var data =
                await _comservice
                    .GetDeliveryHistory(id);

            return Json(data);
        }
    }
}