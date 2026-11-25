using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IModProjectService _modProjectService;
        private readonly IResourcePackService _resourcePackService;
        private readonly IDataPackService _dataPackService;

        public HomeController(
            IModProjectService modProjectService,
            IResourcePackService resourcePackService,
            IDataPackService dataPackService)
        {
            _modProjectService = modProjectService;
            _resourcePackService = resourcePackService;
            _dataPackService = dataPackService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.RecentMods = await _modProjectService.GetPaginatedAsync(null, 1, 4);
            ViewBag.RecentResourcePacks = await _resourcePackService.GetPaginatedAsync(null, 1, 4);
            ViewBag.RecentDataPacks = await _dataPackService.GetPaginatedAsync(null, 1, 4);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }

            return View("Error500");
        }
    }
}