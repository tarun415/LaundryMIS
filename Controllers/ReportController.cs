using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _rptservice;

        public ReportController(IReportService rptservice)
        {
            _rptservice = rptservice;
        }
        //Delivery Report for both provider and Hospital
        public async Task<IActionResult>
DeliverySummaryReport()
        {
            var model =
                await _rptservice
                    .GetDeliverySummaryReport();

            return View(model);
        }
        public async Task<IActionResult>
DeliveryHistory(int id)
        {
            var data =
                await _rptservice
                    .GetDeliveryHistory(id);

            return Json(data);
        }
        public async Task<IActionResult> WeeklyDeliveryReport(DateTime? fromDate, DateTime? toDate)
        {
            fromDate ??= DateTime.Today.AddDays(-7);
            toDate ??= DateTime.Today;

            var model = await _rptservice.WeeklyDeliveryReport(fromDate.Value, toDate.Value);

            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");

            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");

            return View(model);
        }
        public async Task<IActionResult> MonthlyReport(int? year, int? month)
        {
            year ??= DateTime.Now.Year;
            month ??= DateTime.Now.Month;

            var model = await _rptservice.GetMonthlyReport(year.Value, month.Value);

            ViewBag.Year = year;
            ViewBag.Month = month;

            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetMonthlyPickupDetails(int month, int year)
        {
            var data = await _rptservice.GetMonthlyPickupDetails(month, year);
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetDeliveryHistory(int id)
        {
            var result = await _rptservice.GetDeliveryHistory(id);

            return Json(result);
        }
        public async Task<IActionResult> PendingLinenReport()
        {
            var model = await _rptservice.GetPendingLinenReport();
            return View(model);
        }

        public async Task<IActionResult> DeliveryAgingReport()
        {
            var model = await _rptservice.GetDeliveryAgingReport();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveryAgingItems(int id)
        {
            var data = await _rptservice.GetDeliveryAgingDetailItems(id);
            return Json(data);
        }



    }
}
