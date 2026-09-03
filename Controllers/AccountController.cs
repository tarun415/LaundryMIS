using LaudaryMis.Infrastructure;
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

            // Sign in to this tab's slot only, so a sign-in here cannot disturb
            // an account already signed in on another slot.
            var slot = HttpContext.CurrentSlot();
            var scheme = TabSlots.SchemeFor(slot);

            var identity = new ClaimsIdentity(claims, scheme);

            await HttpContext.SignInAsync(
                scheme,
                new ClaimsPrincipal(identity));

            TabSlots.MarkOccupied(HttpContext, slot, true);

            return RedirectToAction("Dashboard", roleName);
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
            // Only this slot signs out; accounts on other slots stay signed in.
            var slot = HttpContext.CurrentSlot();

            await HttpContext.SignOutAsync(TabSlots.SchemeFor(slot));

            TabSlots.MarkOccupied(HttpContext, slot, false);

            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Opens the sign-in page on a different slot, so another account can be
        /// used alongside this one instead of replacing it.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult UseAnotherAccount(int? slot)
        {
            var target = slot ?? NextSlot();

            if (!TabSlots.IsValid(target))
                target = 0;

            // Built by hand rather than with Url.Action: this deliberately
            // leaves the current slot for a different one, and Url.Action would
            // keep reusing the slot already on the request.
            return Redirect($"/{TabSlots.Prefix}/{target}/Account/Login");
        }

        // Picks the first slot nobody is signed in on, falling back to the one
        // after the current slot when they are all taken.
        private int NextSlot()
        {
            var occupied = TabSlots.OccupiedSlots(Request);

            for (int slot = 0; slot < TabSlots.Count; slot++)
            {
                if (!occupied.Contains(slot))
                    return slot;
            }

            return (HttpContext.CurrentSlot() + 1) % TabSlots.Count;
        }
    }
}