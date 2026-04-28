using LaudaryMis.Models;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHospitalService _service;
        private readonly IProviderService _providerService;
        private readonly IAgreementService _agreementService;
        private readonly IWardService _wardService;

        public AdminController(
     IHospitalService service,
     IProviderService providerService,
     IAgreementService agreementService,
     IWardService wardService)
        {
            _service = service;
            _providerService = providerService;
            _agreementService = agreementService;
            _wardService = wardService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        #region Hospital
        // CREATE Hospital
        public IActionResult CreateHospital()
        {
            return View();
        }
        // SAVE Hospital
        [HttpPost]
        public async Task<IActionResult> CreateHospital(HospitalVM model)
        {
            await _service.SaveAsync(model);
            return RedirectToAction("Hospitals");
        }
      
       //Edit
        public async Task<IActionResult> EditHospital(int id)
        {
            var data = await _service.GetHospitalByIdAsync(id);

            return View("CreateHospital", data);
        }

        //Delete
        [HttpPost]
        public async Task<JsonResult> DeleteHospital(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Hospital not found" });

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // LIST
        public async Task<IActionResult> Hospitals()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }
        #endregion

        #region Provider
        // CREATE Provider
        public IActionResult CreateProvider()
        {
            return View();
        }
        // SAVE Provider
        [HttpPost]
        public async Task<IActionResult> CreateProvider(ProvidersVM model)
        {
            await _providerService.SaveAsync(model);
            return RedirectToAction("Providers");
        }
        //Edit
        public async Task<IActionResult> EditProvider(int id)
        {
            var data = await _providerService.GetProviderByIdAsync(id);

            return View("CreateProvider", data);
        }
        //Delete
        [HttpPost]
        public async Task<JsonResult> DeleteProvider(int id)
        {
            try
            {
                var result = await _providerService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Provider not found" });

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // LIST of Provider
        public async Task<IActionResult> Providers()
        {
            var data = await _providerService.GetProviderAsync();
            return View(data);
        }

        #endregion

        #region Agreement
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
            string? filePath = model.FilePath; //  OLD FILE HOLD

            if (model.AgreementFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.AgreementFile.FileName);

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/agreements");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.AgreementFile.CopyToAsync(stream);
                }

                filePath = "/uploads/agreements/" + fileName; // 👈 NEW FILE
            }

            await _agreementService.SaveAsync(model, filePath);

            return RedirectToAction("Agreements");
        }
        public async Task<IActionResult> EditAgreement(int id)
        {
            var data = await _agreementService.GetAgreementByIdAsync(id);

            data.Providers = (await _providerService.GetAll()).ToList();
            data.Hospitals = (await _service.GetAllAsync()).ToList();

            return View("CreateAgreement", data);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteAgreement(int id)
        {
            try
            {
                var result = await _agreementService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Not found" });

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<IActionResult> Agreements()
        {
            var data = await _agreementService.GetAllAsync();
            return View(data);
        }
        #endregion

        #region Ward

        // LIST
        public async Task<IActionResult> Wards()
        {
            var data = await _wardService.GetWardAsync();
            return View(data);
        }

        // CREATE PAGE
        public IActionResult CreateWard()
        {
            return View();
        }

        // SAVE
        [HttpPost]
        public async Task<IActionResult> CreateWard(WardVM model)
        {
            await _wardService.SaveAsync(model);
            return RedirectToAction("Wards");
        }

        // EDIT
        public async Task<IActionResult> EditWard(int id)
        {
            var data = await _wardService.GetWardByIdAsync(id);
            return View("CreateWard", data);
        }

        // DELETE
        [HttpPost]
        public async Task<JsonResult> DeleteWard(int id)
        {
            try
            {
                var result = await _wardService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Ward not found" });

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion











    }
}
