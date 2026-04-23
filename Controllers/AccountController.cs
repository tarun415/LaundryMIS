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

            var user = await _service.Login(model.RoleName, model.Password, model.RoleId);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.FullName ?? ""),
        new Claim(ClaimTypes.Role, user.RoleName ?? ""),
        new Claim("UserId", user.UserId.ToString()),
        new Claim("HospitalId", user.HospitalId?.ToString() ?? ""),
new Claim("ProviderId", user.ProviderId.ToString())  // 🔥 ADD THIS
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            // 🔥 ROLE BASED REDIRECT
            return user.RoleName switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Hospital" => RedirectToAction("Dashboard", "Hospital"),
                "Provider" => RedirectToAction("Dashboard", "Provider"),
                _ => RedirectToAction("Login")
            };
        }
    }
}
