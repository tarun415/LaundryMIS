using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    // Vendor profile aur KYC documents.
    // Provider apna profile dekhta/badalta hai; Admin sirf dekh sakta hai.
    [Authorize]
    public class ProviderProfileController : Controller
    {
        private readonly IProviderProfileService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IContactService _contact;

        public ProviderProfileController(IProviderProfileService service, IWebHostEnvironment env, IContactService contact)
        {
            _service = service;
            _env = env;
            _contact = contact;
        }

        // ──────────────────────────────────────────────────────
        // Provider: apna profile
        // ──────────────────────────────────────────────────────

        [HttpGet]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> Index()
        {
            var profile = await _service.GetProfileAsync(GetProviderId());
            if (profile == null) return NotFound();

            return View(profile);
        }

        [HttpPost]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> Save(ProviderProfileVM model)
        {
            // Provider hamesha login se, form se nahi
            model.ProviderId = GetProviderId();

            // Codes capital mein karke dobara validate (lowercase type karna chalta hai)
            model.GSTNo = Upper(model.GSTNo);
            model.PANNo = Upper(model.PANNo);
            model.BankIFSC = Upper(model.BankIFSC);
            ModelState.Clear();
            TryValidateModel(model);

            if (model.LegalStatus != null && !ProviderProfileVM.LegalStatuses.Contains(model.LegalStatus))
                ModelState.AddModelError(nameof(model.LegalStatus), "Choose the firm type from the list.");

            // The mobile number is also used to sign in, so it must look real and be this vendor's own
            if (ModelState.GetValidationState(nameof(model.Phone)) != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                var phoneError = await _contact.CheckMobileAsync(model.Phone, ownProviderId: model.ProviderId);
                if (phoneError != null)
                    ModelState.AddModelError(nameof(model.Phone), phoneError);
            }

            if (!ModelState.IsValid)
            {
                var current = await _service.GetProfileAsync(model.ProviderId);
                model.Email = current?.Email;
                model.Documents = current?.Documents ?? new();
                model.UpdatedOn = current?.UpdatedOn;
                TempData["Error"] = "Some fields are not valid; see below.";
                return View("Index", model);
            }

            await _service.SaveProfileAsync(model, GetUserId());

            TempData["Success"] = "Profile saved.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> Upload(
            string documentType, string? documentNo, DateTime? validTill, IFormFile? file)
        {
            int providerId = GetProviderId();

            var type = DocumentTypes.Find(documentType);
            string? error = type == null
                ? "Choose the document type from the list."
                : await UploadValidator.ValidateAsync(file, UploadValidator.DocumentExtensions);

            if (error == null && type!.HasExpiry && validTill == null)
                error = $"Enter the validity (valid till) date for {type.Name}.";

            if (error == null && !string.IsNullOrEmpty(documentNo)
                && !System.Text.RegularExpressions.Regex.IsMatch(documentNo, @"^[A-Za-z0-9 /\-.]{1,100}$"))
                error = "The document number may only contain letters, numbers, spaces and / - .";

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(Index), null, "documents");
            }

            // Files wwwroot ke bahar — sirf Document action se, login ke baad hi khulti hain
            var folder = ProviderFolder(providerId);
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file!.FileName).ToLowerInvariant();
            var storedName = $"{Guid.NewGuid():N}{ext}";

            using (var stream = new FileStream(Path.Combine(folder, storedName), FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            await _service.AddDocumentAsync(new ProviderDocumentVM
            {
                ProviderId = providerId,
                DocumentType = type!.Name,
                DocumentNo = string.IsNullOrWhiteSpace(documentNo) ? null : documentNo.Trim(),
                ValidTill = type.HasExpiry ? validTill?.Date : null,
                FileName = storedName,
                OriginalFileName = Path.GetFileName(file.FileName),
                ContentType = ext == ".pdf" ? "application/pdf" : ext == ".png" ? "image/png" : "image/jpeg",
                FileSize = file.Length
            }, GetUserId());

            TempData["Success"] = $"{type.Name} uploaded.";
            return RedirectToAction(nameof(Index), null, "documents");
        }

        [HttpPost]
        [Authorize(Roles = "ServiceProvider")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var ok = await _service.DeleteDocumentAsync(id, GetProviderId());

            if (ok) TempData["Success"] = "Document removed.";
            else TempData["Error"] = "Document not found.";

            return RedirectToAction(nameof(Index), null, "documents");
        }

        // ──────────────────────────────────────────────────────
        // Document kholna — apna (Provider) ya koi bhi (Admin)
        // ──────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Document(int id)
        {
            var doc = await _service.GetDocumentAsync(id);
            if (doc == null) return NotFound();

            bool allowed = User.IsInRole("Admin")
                || (User.IsInRole("ServiceProvider") && GetClaimInt("ProviderId") == doc.ProviderId);
            if (!allowed) return Forbid();

            var path = Path.Combine(ProviderFolder(doc.ProviderId), Path.GetFileName(doc.FileName));
            if (!System.IO.File.Exists(path)) return NotFound();

            // Browser mein dikhao, download name asli file ka
            var downloadName = string.IsNullOrWhiteSpace(doc.OriginalFileName) ? doc.FileName : doc.OriginalFileName;
            Response.Headers["Content-Disposition"] =
                new System.Net.Mime.ContentDisposition { Inline = true, FileName = downloadName }.ToString();
            Response.Headers["X-Content-Type-Options"] = "nosniff";

            return PhysicalFile(path, doc.ContentType ?? "application/octet-stream");
        }

        // ──────────────────────────────────────────────────────
        // Admin: kisi bhi vendor ka profile (sirf dekhna)
        // ──────────────────────────────────────────────────────

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _service.GetProfileAsync(id);
            if (profile == null) return NotFound();

            profile.ReadOnly = true;
            return View("Index", profile);
        }

        // ──────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────

        private string ProviderFolder(int providerId) =>
            Path.Combine(_env.ContentRootPath, "App_Data", "ProviderDocuments", providerId.ToString());

        private int GetProviderId()
        {
            var id = GetClaimInt("ProviderId");
            if (id <= 0) throw new UnauthorizedAccessException("Provider ID not found.");
            return id;
        }

        private int GetUserId() => GetClaimInt(ClaimTypes.NameIdentifier);

        private int GetClaimInt(string type) =>
            int.TryParse(User.FindFirst(type)?.Value, out var v) ? v : 0;

        private static string? Upper(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    }
}
