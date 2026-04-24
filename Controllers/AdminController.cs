using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HospitalService _service;
        private readonly ProviderService _providerService;
        private readonly AgreementService _agreementService;


        public AdminController(
        HospitalService service,
        ProviderService providerService,
        AgreementService agreementService)
        {
            _service = service;
            _providerService = providerService;
            _agreementService = agreementService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // LIST
        public async Task<IActionResult> Hospitals()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // CREATE PAGE
        public IActionResult CreateHospital()
        {
            return View();
        }

        // SAVE
        [HttpPost]
        public async Task<IActionResult> CreateHospital(HospitalVM model)
        {
            await _service.SaveAsync(model);
            return RedirectToAction("Hospitals");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAgreement()
        {
            var vm = new AgreementVM();

            vm.Providers = (await _providerService.GetAll()).ToList();
            vm.Hospitals = (await _service.GetAllAsync()).ToList();

            vm.StartDate = DateTime.Now;

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAgreement(AgreementVM model)
        {
            string? filePath = null;

            if (model.AgreementFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.AgreementFile.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/agreements", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await model.AgreementFile.CopyToAsync(stream);

                filePath = "/uploads/agreements/" + fileName;
            }

            await _agreementService.SaveAsync(model, filePath);

            return RedirectToAction("CreateAgreement");
        }



        // LIST of Provider
        public async Task<IActionResult> Providers()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }
        // CREATE Provider
        public IActionResult CreateProvider()
        {
            return View();
        }
    }
}
