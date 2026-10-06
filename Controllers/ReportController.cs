using LaudaryMis.Helpers;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _rptservice;
        private readonly IAccessGuard _guard;

        public ReportController(IReportService rptservice, IAccessGuard guard)
        {
            _rptservice = rptservice;
            _guard = guard;
        }

        // Admin sees every pickup; a hospital or vendor only the pickups of its own contract.
        private async Task<List<T>> OwnOnly<T>(List<T> rows, Func<T, int> pickupId)
        {
            var visible = await _guard.VisiblePickupIdsAsync(User);

            return visible == null
                ? rows
                : rows.Where(x => visible.Contains(pickupId(x))).ToList();
        }

        // Totals for the report pages: all hospitals for admin, otherwise only the signed-in
        // hospital or vendor. A login without its id claim gets an id that matches nothing.
        private (int? HospitalId, int? ProviderId) Scope() =>
            User.IsAdmin() ? (null, null)
            : User.IsInRole("Hospital") ? (User.HospitalId() ?? -1, null)
            : User.IsInRole("Provider") ? (null, User.ProviderId() ?? -1)
            : (-1, null);

        //Delivery Report for both provider and Hospital
        public async Task<IActionResult>DeliverySummaryReport()
        {
            var model =  await _rptservice .GetDeliverySummaryReport();
            return View(await OwnOnly(model, x => x.PickupId));
        }
        public async Task<IActionResult>
DeliveryHistory(int id)
        {
            if (!await _guard.CanAccessPickupAsync(User, id)) return Forbid();

            var data =
                await _rptservice
                    .GetDeliveryHistory(id);

            return Json(data);
        }
        public async Task<IActionResult> WeeklyDeliveryReport(DateTime? fromDate, DateTime? toDate)
        {
            fromDate ??= DateTime.Today.AddDays(-7);
            toDate ??= DateTime.Today;
            var (hospitalId, providerId) = Scope();
            var model = await _rptservice.WeeklyDeliveryReport(fromDate.Value, toDate.Value, hospitalId, providerId);
            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");
            return View(model);
        }

        public async Task<IActionResult> MonthlyReport(int? year, int? month)
        {
            year ??= DateTime.Now.Year;
            month ??= DateTime.Now.Month;

            var (hospitalId, providerId) = Scope();
            var model = await _rptservice.GetMonthlyReport(year.Value, month.Value, hospitalId, providerId);

            ViewBag.Year = year;
            ViewBag.Month = month;

            return View(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetMonthlyPickupDetails(int month, int year)
        {
            var data = await _rptservice.GetMonthlyPickupDetails(month, year);
            return Json(await OwnOnly(data, x => x.PickupId));
        }
        [HttpGet]
        public async Task<IActionResult> GetDeliveryHistory(int id)
        {
            if (!await _guard.CanAccessPickupAsync(User, id)) return Forbid();

            var result = await _rptservice.GetDeliveryHistory(id);

            return Json(result);
        }
        public async Task<IActionResult> PendingLinenReport()
        {
            var model = await _rptservice.GetPendingLinenReport();
            return View(await OwnOnly(model, x => x.PickupId));
        }

        public async Task<IActionResult> DeliveryAgingReport()
        {
            var model = await _rptservice.GetDeliveryAgingReport(
                await _guard.VisiblePickupIdsAsync(User));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveryAgingItems(int id)
        {
            if (!await _guard.CanAccessPickupAsync(User, id)) return Forbid();

            var data = await _rptservice.GetDeliveryAgingDetailItems(id);
            return Json(data);
        }



    }
}
