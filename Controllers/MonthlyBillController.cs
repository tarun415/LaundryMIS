using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    [Authorize]
    public class MonthlyBillController : Controller
    {
        private readonly IMonthlyBillService _billService;
        private readonly IDailyService _dailyService;

        public MonthlyBillController(IMonthlyBillService billService, IDailyService dailyService)
        {
            _billService = billService;
            _dailyService = dailyService;
        }

        // ════════════════════════════════════════════════
        // PROVIDER ACTIONS
        // ════════════════════════════════════════════════

        // GET /MonthlyBill/SelectHospital
        [Authorize(Roles = "Provider")]
        [HttpGet]
        public async Task<IActionResult> SelectHospital()
        {
            var hospitals = await _dailyService.GetHospitalsByProvider(GetProviderId());
            ViewBag.Hospitals = hospitals;
            ViewBag.CurrentMonth = DateTime.Now.Month;
            ViewBag.CurrentYear = DateTime.Now.Year;
            return View();
        }

        // GET /MonthlyBill/Create?hospitalId=1&month=4&year=2026
        [Authorize(Roles = "Provider")]
        [HttpGet]
        public async Task<IActionResult> Create(
            int hospitalId, int month, int year)
        {
            try
            {
                int providerId = GetProviderId();
                var vm = await _billService.LoadProviderBillFormAsync(
                    providerId, hospitalId, month, year);
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyBills));
            }
        }

        // POST /MonthlyBill/SaveDraft
        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(MonthlyBillVM model)
        {
            if (!ModelState.IsValid)
            {
                _billService.ComputeAmounts(model);
                return View("Create", model);
            }

            var (success, message, billId) =
                await _billService.SaveDraftAsync(model, GetUserId());

            if (!success)
            {
                TempData["Error"] = message;
                _billService.ComputeAmounts(model);
                return View("Create", model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Detail), new { id = billId });
        }

        // POST /MonthlyBill/SubmitToHospital
        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitToHospital(int billId)
        {
            var (success, message) =
                await _billService.SubmitToHospitalAsync(
                    billId, GetProviderId());

            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Detail), new { id = billId });
        }

        // GET /MonthlyBill/MyBills  (Provider ke apne bills)
        [Authorize(Roles = "Provider")]
        [HttpGet]
        public async Task<IActionResult> MyBills()
        {
            var bills = await _billService.GetProviderBillsAsync(
                GetProviderId());
            return View("ProviderBills", bills);
        }

        // ════════════════════════════════════════════════
        // HOSPITAL ACTIONS
        // ════════════════════════════════════════════════

        // GET /MonthlyBill/HospitalQueue  (Hospital ke verify-pending bills)
        [Authorize(Roles = "Hospital")]
        [HttpGet]
        public async Task<IActionResult> HospitalQueue()
        {
            var bills = await _billService.GetBillsForHospitalVerifyAsync(
                GetHospitalId());
            return View("HospitalQueue", bills);
        }

        // POST /MonthlyBill/HospitalAction
        [Authorize(Roles = "Hospital")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HospitalAction(
            int billId, bool approve, string? remarks)
        {
            var (success, message) = await _billService.HospitalActionAsync(
                billId, GetUserId(), approve, remarks);

            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Detail), new { id = billId });
        }

        // ════════════════════════════════════════════════
        // ADMIN / CMS ACTIONS
        // ════════════════════════════════════════════════

        // GET /MonthlyBill/Index
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(
            string? status = null, int? hospitalId = null)
        {
            var bills = await _billService.GetAllBillsAsync(
                status, hospitalId);
            ViewBag.FilterStatus = status;
            ViewBag.FilterHospital = hospitalId;
            return View(bills);
        }

        // POST /MonthlyBill/CMSAction
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CMSAction(
            int billId, bool approve, string? remarks)
        {
            var (success, message) = await _billService.CMSActionAsync(
                billId, GetUserId(), approve, remarks);

            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Detail), new { id = billId });
        }

        // ════════════════════════════════════════════════
        // SHARED
        // ════════════════════════════════════════════════

        // GET /MonthlyBill/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var vm = await _billService.GetBillDetailAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        // POST /MonthlyBill/Recalculate  (AJAX)
        [HttpPost]
        public IActionResult Recalculate([FromBody] MonthlyBillVM model)
        {
            _billService.ComputeAmounts(model);
            return Json(new
            {
                annualExGST = model.AnnualValueExGST.ToString("N2"),
                annualInGST = model.AnnualValueInGST.ToString("N2"),
                monthlyGross = model.MonthlyGrossAmount.ToString("N2"),
                paymentBand = model.PaymentBandPercent,
                basePayable = model.BasePayableAmount.ToString("N2"),
                tdsAmount = model.TDSAmount.ToString("N2"),
                netPayable = model.NetPayableAmount.ToString("N2")
            });
        }

        // ════════════════════════════════════════════════
        // HELPERS
        // ════════════════════════════════════════════════
        private int GetUserId()
        {
            // AccountController mein NameIdentifier claim ADD karna zaroori hai!
            var val = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(val, out int id))
                throw new Exception("User ID claim nahi mila. " +
                    "AccountController mein NameIdentifier claim add karo.");
            return id;
        }

        private int GetProviderId()
        {
            var val = User.FindFirst("ProviderId")?.Value;
            if (!int.TryParse(val, out int id) || id <= 0)
                throw new UnauthorizedAccessException("Provider ID nahi mila.");
            return id;
        }

        private int GetHospitalId()
        {
            var val = User.FindFirst("HospitalId")?.Value;
            if (!int.TryParse(val, out int id) || id <= 0)
                throw new UnauthorizedAccessException("Hospital ID nahi mila.");
            return id;
        }
    }
}