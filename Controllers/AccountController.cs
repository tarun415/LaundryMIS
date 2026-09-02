using LaudaryMis.Models;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaudaryMis.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;

        public AccountController(IUserService service)
        {
            _service = service;
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

            User? user = null;
            LoginResult result = null;
            if (model.RoleId == 1)
            {

                result = await _service.Login(model.Username ?? "", model.Password, model.RoleId);

            }
            else if (model.RoleId == 2)
            {
                result = await _service.LoginHospital(model.HospitalId, model.Password);
            }
            else if (model.RoleId == 3)
            {
                result = await _service.LoginProvider(model.ProviderId, model.Password);
            }                
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            user = result.User;

            // Derive a canonical role from the RoleId that was actually used to
            // authenticate. Relying on the free-text RoleName coming back from the
            // database is fragile (casing / whitespace / stale values) and was
            // causing Hospital and Provider users to occasionally land on the
            // wrong dashboard. RoleId is authoritative here: 1 = Admin,
            // 2 = Hospital, 3 = Provider.
            var roleName = model.RoleId switch
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

            return RedirectToAction("Dashboard", roleName);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}