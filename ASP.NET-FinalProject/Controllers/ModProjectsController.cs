using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Controllers
{
    public class ModProjectsController : Controller
    {
        private readonly IModProjectService _modProjectService;

        public ModProjectsController(IModProjectService modProjectService)
        {
            _modProjectService = modProjectService;
        }

        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var mods = await _modProjectService.GetAllAsync(searchString);
            return View(mods);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(ModProject modProject)
        {
            ModelState.Remove("Category");
            ModelState.Remove("GameVersion");
            ModelState.Remove("ModLoader");

            if (ModelState.IsValid)
            {
                await _modProjectService.CreateAsync(modProject);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(modProject);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var modProject = await _modProjectService.GetByIdWithDetailsAsync(id.Value);
            if (modProject == null) return NotFound();

            return View(modProject);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var modProject = await _modProjectService.GetByIdAsync(id.Value);
            if (modProject == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(modProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, ModProject modProject)
        {
            if (id != modProject.Id) return NotFound();

            ModelState.Remove("Category");
            ModelState.Remove("GameVersion");
            ModelState.Remove("ModLoader");

            if (ModelState.IsValid)
            {
                try
                {
                    await _modProjectService.UpdateAsync(modProject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _modProjectService.ExistsAsync(modProject.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(modProject);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var modProject = await _modProjectService.GetByIdWithDetailsAsync(id.Value);
            if (modProject == null) return NotFound();

            return View(modProject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _modProjectService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Categories = await _modProjectService.GetCategoriesAsync();
            ViewBag.GameVersions = await _modProjectService.GetGameVersionsAsync();
            ViewBag.ModLoaders = await _modProjectService.GetModLoadersAsync();
        }
    }
}