using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Services
{
    public class ModProjectService : IModProjectService
    {
        private readonly ApplicationDbContext _context;

        public ModProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ModProject>> GetAllAsync(string? searchString = null)
        {
            var mods = _context.ModProjects
                .Include(m => m.Category)
                .Include(m => m.ModLoader)
                .Include(m => m.GameVersion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                mods = mods.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            return await mods.ToListAsync();
        }

        public async Task<ModProject?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.ModProjects
                .Include(m => m.Category)
                .Include(m => m.GameVersion)
                .Include(m => m.ModLoader)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ModProject?> GetByIdAsync(int id)
        {
            return await _context.ModProjects.FindAsync(id);
        }

        public async Task CreateAsync(ModProject modProject)
        {
            _context.ModProjects.Add(modProject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ModProject modProject)
        {
            _context.ModProjects.Update(modProject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var modProject = await _context.ModProjects.FindAsync(id);
            if (modProject != null)
            {
                _context.ModProjects.Remove(modProject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ModProjects.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<GameVersion>> GetGameVersionsAsync()
        {
            return await _context.GameVersions.ToListAsync();
        }

        public async Task<IEnumerable<ModLoader>> GetModLoadersAsync()
        {
            return await _context.ModLoaders.ToListAsync();
        }
    }
}