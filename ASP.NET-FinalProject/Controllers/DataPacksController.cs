using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Controllers
{
    public class DataPacksController : Controller
    {
        private readonly IDataPackService _dataPackService;

        public DataPacksController(IDataPackService dataPackService)
        {
            _dataPackService = dataPackService;
        }

        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var dataPacks = await _dataPackService.GetAllAsync(searchString);
            return View(dataPacks);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _dataPackService.GetByIdWithDetailsAsync(id.Value);
            if (dataPack == null) return NotFound();

            return View(dataPack);
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
        public async Task<IActionResult> Create(DataPack dataPack)
        {
            ModelState.Remove("DataPackCategory");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                await _dataPackService.CreateAsync(dataPack);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(dataPack);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _dataPackService.GetByIdAsync(id.Value);
            if (dataPack == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(dataPack);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, DataPack dataPack)
        {
            if (id != dataPack.Id) return NotFound();

            ModelState.Remove("DataPackCategory");
            ModelState.Remove("GameVersion");

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataPackService.UpdateAsync(dataPack);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataPackService.ExistsAsync(dataPack.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(dataPack);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dataPack = await _dataPackService.GetByIdWithDetailsAsync(id.Value);
            if (dataPack == null) return NotFound();

            return View(dataPack);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dataPackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Categories = await _dataPackService.GetCategoriesAsync();
            ViewBag.GameVersions = await _dataPackService.GetGameVersionsAsync();
        }
    }
}