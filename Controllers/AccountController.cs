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

            if (model.RoleId == 1)
                user = await _service.Login(model.Username ?? "", model.Password, model.RoleId);

            else if (model.RoleId == 2)
                user = await _service.LoginHospital(model.HospitalId, model.Password);

            else if (model.RoleId == 3)
                user = await _service.LoginProvider(model.ProviderId, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),  // ← YEH ADD KARO
    new Claim(ClaimTypes.Name, user.FullName ?? ""),
    new Claim(ClaimTypes.Role, user.RoleName ?? ""),
    new Claim("HospitalId", user.HospitalId?.ToString() ?? ""),
    new Claim("ProviderId", user.ProviderId?.ToString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Dashboard", user.RoleName);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}