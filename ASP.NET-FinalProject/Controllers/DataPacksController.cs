using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Controllers
{
    public class DataPacksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataPacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dataPacks = await _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .ToListAsync();

            return View(dataPacks);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dataPack == null) return NotFound();

            return View(dataPack);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.DataPackCategories.ToList();
            ViewBag.GameVersions = _context.GameVersions.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataPack dataPack)
        {
            ModelState.Remove("DataPackCategory");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                _context.Add(dataPack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.DataPackCategories.ToList();
            ViewBag.GameVersions = _context.GameVersions.ToList();
            return View(dataPack);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _context.DataPacks.FindAsync(id);
            if (dataPack == null) return NotFound();

            ViewBag.Categories = _context.DataPackCategories.ToList();
            ViewBag.GameVersions = _context.GameVersions.ToList();
            return View(dataPack);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DataPack dataPack)
        {
            if (id != dataPack.Id) return NotFound();

            ModelState.Remove("DataPackCategory");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataPack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DataPacks.Any(e => e.Id == dataPack.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.DataPackCategories.ToList();
            ViewBag.GameVersions = _context.GameVersions.ToList();
            return View(dataPack);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dataPack == null) return NotFound();

            return View(dataPack);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataPack = await _context.DataPacks.FindAsync(id);
            if (dataPack != null)
            {
                _context.DataPacks.Remove(dataPack);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
