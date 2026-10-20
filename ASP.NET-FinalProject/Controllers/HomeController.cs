using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.RecentMods = await _context.ModProjects
                .Include(m => m.Category)
                .OrderByDescending(m => m.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RecentResourcePacks = await _context.ResourcePacks
                .Include(r => r.ResourceCategory)
                .OrderByDescending(r => r.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RecentDataPacks = await _context.DataPacks
                .Include(d => d.DataPackCategory)
                .OrderByDescending(d => d.Id)
                .Take(4)
                .ToListAsync();

            return View();
        }
    }
}
