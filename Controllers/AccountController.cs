using LaudaryMis.Helpers;
using LaudaryMis.Models;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;
        private readonly LoginAttemptTracker _attempts;
        private readonly IActivationCodeService _activation;

        public AccountController(IUserService service, LoginAttemptTracker attempts, IActivationCodeService activation)
        {
            _service = service;
            _attempts = attempts;
            _activation = activation;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var account = model.RoleId switch
            {
                1 => "admin:" + (model.Username ?? "").Trim().ToLowerInvariant(),
                2 => "hospital:" + model.HospitalId,
                3 => "provider:" + model.ProviderId,
                _ => "other"
            };

            if (_attempts.IsLocked(account, ip, out var remaining))
            {
                ModelState.AddModelError("",
                    $"Bahut zyada galat koshishein. {Math.Ceiling(remaining.TotalMinutes)} minute baad dobara try karein.");
                return View(model);
            }

            User? user = null;
            LoginResult? result = model.RoleId switch
            {
                1 => await _service.Login(model.Username ?? "", model.Password, model.RoleId),
                2 => await _service.LoginHospital(model.HospitalId, model.Password),
                3 => await _service.LoginProvider(model.ProviderId, model.Password),
                _ => null
            };

            if (result == null)
            {
                ModelState.AddModelError("", "Please select a valid role.");
                return View(model);
            }

            if (!result.Success)
            {
                _attempts.RecordFailure(account, ip);
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            _attempts.Reset(account);
            user = result.User;

            var roleName = await SignInUser(user!, model.RoleId);

            return RedirectToAction("Dashboard", roleName);
        }

        // ──────────────────────────────────────────────────────
        // Register with an activation code — for hospitals / vendors that are
        // already on the tender list. Picks the listed record and attaches a login.
        // ──────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Claim(int roleId = 2)
        {
            return View(new ClaimVM { RoleId = roleId == 3 ? 3 : 2 });
        }

        // Dropdown data for the Claim page: only entries that have no login yet.
        [HttpGet]
        public async Task<IActionResult> UnclaimedHospitals(int districtId) =>
            Json(await _activation.GetUnclaimedHospitalsAsync(districtId));

        [HttpGet]
        public async Task<IActionResult> UnclaimedProviders() =>
            Json(await _activation.GetUnclaimedProvidersAsync());

        [HttpPost]
        public async Task<IActionResult> Claim(ClaimVM model)
        {
            // Doosre role ke chhupe hue fields ko na save karo
            if (model.RoleId == 3) { model.HospitalId = null; model.DistrictId = null; }
            else model.ProviderId = null;

            if (!ModelState.IsValid)
                return View(model);

            int? entityId = model.RoleId == 2 ? model.HospitalId : model.ProviderId;
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Wrong codes are counted per hospital / vendor and per IP (same limits as login)
            var account = $"claim:{model.RoleId}:{entityId}";
            if (_attempts.IsLocked(account, "claim-" + ip, out var remaining))
            {
                ModelState.AddModelError("",
                    $"Bahut zyada galat koshishein. {Math.Ceiling(remaining.TotalMinutes)} minute baad dobara try karein.");
                return View(model);
            }

            var result = await _activation.ClaimAsync(model);

            if (!result.Success || result.User == null)
            {
                if (result.BadCode)
                    _attempts.RecordFailure(account, "claim-" + ip);

                ModelState.AddModelError("", result.Message);
                model.Code = string.Empty;
                return View(model);
            }

            _attempts.Reset(account);

            var roleName = await SignInUser(result.User, model.RoleId);

            TempData["Success"] = "Registration ho gaya. Laundry MIS mein aapka swagat hai!";
            return RedirectToAction("Dashboard", roleName);
        }

        // ──────────────────────────────────────────────────────
        // Self-registration (Hospital / Vendor) — turant login,
        // Admin approval nahi chahiye
        // ──────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register(int roleId = 2)
        {
            return View(new RegisterVM { RoleId = roleId == 3 ? 3 : 2 });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            // Doosre role ke chhupe hue fields ko na save karo, na validate
            string[] otherRoleFields = model.RoleId == 3
                ? new[] { nameof(model.HospitalName), nameof(model.DistrictId), nameof(model.ContactPerson), nameof(model.Address) }
                : new[] { nameof(model.ProviderName), nameof(model.FirmName) };

            foreach (var field in otherRoleFields)
                ModelState.Remove(field);

            if (model.RoleId == 3)
            {
                model.HospitalName = model.ContactPerson = model.Address = null;
                model.DistrictId = null;
            }
            else
            {
                model.ProviderName = model.FirmName = null;
            }

            if (model.RoleId == 2)
            {
                if (string.IsNullOrWhiteSpace(model.HospitalName))
                    ModelState.AddModelError(nameof(model.HospitalName), "Hospital ka naam daalein.");
                if (model.DistrictId is null or <= 0)
                    ModelState.AddModelError(nameof(model.DistrictId), "District chunein.");
                if (string.IsNullOrWhiteSpace(model.ContactPerson))
                    ModelState.AddModelError(nameof(model.ContactPerson), "Contact person ka naam daalein.");
            }
            else if (model.RoleId == 3)
            {
                if (string.IsNullOrWhiteSpace(model.FirmName))
                    ModelState.AddModelError(nameof(model.FirmName), "Firm / company ka naam daalein.");
                if (string.IsNullOrWhiteSpace(model.ProviderName))
                    ModelState.AddModelError(nameof(model.ProviderName), "Contact person ka naam daalein.");
            }

            if (!ModelState.IsValid)
                return View(model);

            // Ek IP se 15 minute mein 5 se zyada registration nahi
            // (login lockout wala tracker hi, alag key ke saath).
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var throttleKey = "register:" + ip;
            if (_attempts.IsLocked(throttleKey, "register-" + ip, out var remaining))
            {
                ModelState.AddModelError("",
                    $"Bahut zyada registration. {Math.Ceiling(remaining.TotalMinutes)} minute baad dobara try karein.");
                return View(model);
            }

            var result = model.RoleId == 2
                ? await _service.RegisterHospital(model)
                : await _service.RegisterProvider(model);

            if (!result.Success || result.User == null)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            _attempts.RecordFailure(throttleKey, "register-" + ip);

            var roleName = await SignInUser(result.User, model.RoleId);

            TempData["Success"] = "Registration ho gaya. Laundry MIS mein aapka swagat hai!";
            return RedirectToAction("Dashboard", roleName);
        }

        // Derive a canonical role from the RoleId that was actually used to
        // authenticate. Relying on the free-text RoleName coming back from the
        // database is fragile (casing / whitespace / stale values) and was
        // causing Hospital and Provider users to occasionally land on the
        // wrong dashboard. RoleId is authoritative here: 1 = Admin,
        // 2 = Hospital, 3 = Provider.
        private async Task<string> SignInUser(User user, int roleId)
        {
            var roleName = roleId switch
            {
                1 => "Admin",
                2 => "Hospital",
                3 => "Provider",
                _ => user.RoleName ?? ""
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("HospitalId", user.HospitalId?.ToString() ?? ""),
                new Claim("ProviderId", user.ProviderId?.ToString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return roleName;
        }

        // Lets an already-rendered page detect that the auth cookie now belongs
        // to a different user, which happens when someone signs in as another
        // role in a second tab of the same browser (the cookie is shared).
        [AllowAnonymous]
        [HttpGet]
        public IActionResult WhoAmI()
        {
            if (User?.Identity?.IsAuthenticated != true)
                return Json(new { authenticated = false, id = "", role = "", name = "" });

            return Json(new
            {
                authenticated = true,
                id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                role = User.FindFirst(ClaimTypes.Role)?.Value ?? "",
                name = User.Identity?.Name ?? ""
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}