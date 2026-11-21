using ASP.NET_FinalProject.Areas.Admin.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly IModProjectService _modService;
        private readonly IResourcePackService _resourcePackService;
        private readonly IDataPackService _dataPackService;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(
            IModProjectService modService,
            IResourcePackService resourcePackService,
            IDataPackService dataPackService,
            UserManager<IdentityUser> userManager)
        {
            _modService = modService;
            _resourcePackService = resourcePackService;
            _dataPackService = dataPackService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                ModsCount = (await _modService.GetAllAsync()).Count(),
                ResourcePacksCount = (await _resourcePackService.GetAllAsync()).Count(),
                DataPacksCount = (await _dataPackService.GetAllAsync()).Count(),
                UsersCount = await _userManager.Users.CountAsync()
            };

            return View(model);
        }
    }
}
