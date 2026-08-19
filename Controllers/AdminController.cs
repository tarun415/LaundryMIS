using LaudaryMis.Models;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHospitalService _service;
        private readonly IProviderService _providerService;
        private readonly IAgreementService _agreementService;
        private readonly IWardService _wardService;
        private readonly ILocationService _locationService;

        public AdminController(
            IHospitalService service,
            IProviderService providerService,
            IAgreementService agreementService,
            IWardService wardService,
            ILocationService locationService)
        {
            _service = service;
            _providerService = providerService;
            _agreementService = agreementService;
            _wardService = wardService;
            _locationService = locationService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        #region Hospital
        // LIST
        public async Task<IActionResult> Hospitals()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }
        // CREATE Hospital
        public IActionResult CreateHospital()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHospital(HospitalVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
           .SelectMany(v => v.Errors)
           .Select(e => e.ErrorMessage)
           .ToList();
                return View(model);
            }

            try
            {
                await _service.CreateHospitalWithLogin(model);

                TempData["SuccessMessage"] = "Hospital saved successfully.";

                return RedirectToAction("Hospitals");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                // Handle SQL UNIQUE constraint / duplicate key
                var message = GetDuplicateKeyMessage(ex);

                ModelState.AddModelError("", message);

                return View(model);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                // Any other SQL exception
                ModelState.AddModelError(
                    "",
                    "Unable to save the hospital due to a database error. Please try again."
                );

                return View(model);
            }
            catch (Exception)
            {
                // Any unexpected exception
                ModelState.AddModelError(
                    "",
                    "Something went wrong while saving the hospital. Please try again."
                );

                return View(model);
            }
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
       

        #endregion

        #region Provider
        // CREATE Provider
        public IActionResult CreateProvider()
        {
            return View();
        }
        //// SAVE Provider
        //[HttpPost]
        //public async Task<IActionResult> CreateProvider(ProvidersVM model)
        //{
        //   // await _providerService.SaveAsync(model);
        //    await _providerService.CreateProviderWithLogin(model);
        //    return RedirectToAction("Providers");
        //}
        // SAVE Provider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProvider(ProvidersVM model)
        {
            bool isEdit = model.ProviderId > 0;

            if (!isEdit && string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(
                    nameof(model.Password),
                    "Password is required."
                );

                return View(model);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            try
            {
                await _providerService.CreateProviderWithLogin(model);

                TempData["SuccessMessage"] = isEdit
                    ? "Provider updated successfully."
                    : "Provider saved successfully.";

                return RedirectToAction("Providers");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                var message = GetDuplicateKeyMessage(ex);

                TempData["ErrorMessage"] = message;

                return View(model);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                TempData["ErrorMessage"] =
                    "Unable to save the provider due to a database error. Please try again.";

                return View(model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "Something went wrong while saving the provider. Please try again.";

                return View(model);
            }
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
            var model = new WardVM
            {
                IsActive = true
            };
            return View(model);
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

        #region Location 
        [HttpGet]
        public async Task<JsonResult> GetStates()
        {
            var data = await _locationService.GetStates();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDistricts(int stateId)
        {
            var data = await _locationService.GetDistrictsByState(stateId);
            return Json(data);
        }

        #endregion

        private string GetDuplicateKeyMessage(
    Microsoft.Data.SqlClient.SqlException ex)
        {
            var message = ex.Message.ToLowerInvariant();

            if (message.Contains("email"))
            {
                return "This email address is already registered. Please use a different email address.";
            }

            if (message.Contains("phone") ||
                message.Contains("mobile"))
            {
                return "This mobile number is already registered. Please use a different mobile number.";
            }

            return "The information you entered already exists. Please use different details.";
        }

    }
}
