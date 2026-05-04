using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaudaryMis.Controllers
{
    [Authorize(Roles = "Provider")]
    public class ProviderController : Controller
    {
        private readonly IDailyService _service;
        private readonly IProviderService _ProviderService;
        private readonly IWPRService _wprService;

        public ProviderController(
            IDailyService service,
            IProviderService providerService,
            IWPRService wprService)
        {
            _service = service;
            _ProviderService = providerService;
            _wprService = wprService;
        }

        private int GetProviderId()
        {
            var claim = User.FindFirst("ProviderId")?.Value;
            if (!int.TryParse(claim, out int id) || id <= 0)
                throw new UnauthorizedAccessException();

            return id;
        }

        public IActionResult Dashboard() => View();

        // 🔥 WPR GET
      
    }
}