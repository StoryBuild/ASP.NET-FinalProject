using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Controllers
{
    public class ResourcePacksController : Controller
    {
        private readonly IResourcePackService _resourcePackService;

        public ResourcePacksController(IResourcePackService resourcePackService)
        {
            _resourcePackService = resourcePackService;
        }

        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            const int pageSize = 6;
            var packs = await _resourcePackService.GetPaginatedAsync(searchString, pageNumber, pageSize);
            return View(packs);
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
        public async Task<IActionResult> Create(ResourcePack resourcePack)
        {
            ModelState.Remove("ResourceCategory");
            ModelState.Remove("ResourceFeature");
            ModelState.Remove("ResourceResolution");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                await _resourcePackService.CreateAsync(resourcePack);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(resourcePack);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pack = await _resourcePackService.GetByIdWithDetailsAsync(id.Value);
            if (pack == null) return NotFound();

            return View(pack);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pack = await _resourcePackService.GetByIdAsync(id.Value);
            if (pack == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(pack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, ResourcePack resourcePack)
        {
            if (id != resourcePack.Id) return NotFound();

            ModelState.Remove("ResourceCategory");
            ModelState.Remove("ResourceFeature");
            ModelState.Remove("ResourceResolution");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                try
                {
                    await _resourcePackService.UpdateAsync(resourcePack);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _resourcePackService.ExistsAsync(resourcePack.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(resourcePack);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pack = await _resourcePackService.GetByIdWithDetailsAsync(id.Value);
            if (pack == null) return NotFound();

            return View(pack);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _resourcePackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Categories = await _resourcePackService.GetCategoriesAsync();
            ViewBag.Features = await _resourcePackService.GetFeaturesAsync();
            ViewBag.Resolutions = await _resourcePackService.GetResolutionsAsync();
            ViewBag.GameVersions = await _resourcePackService.GetGameVersionsAsync();
        }
    }
}