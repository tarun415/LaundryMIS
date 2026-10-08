using System.Diagnostics;
using System.Security.Claims;
using LaudaryMis.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // The welcome page for visitors; someone who is already signed in goes straight to their dashboard
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role is "Admin" or "Hospital" or "ServiceProvider" or "CMS")
                    return RedirectToAction("Dashboard", role == "ServiceProvider" ? "Provider" : role);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
