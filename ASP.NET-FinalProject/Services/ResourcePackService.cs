using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Services
{
    public class ResourcePackService : IResourcePackService
    {
        private readonly ApplicationDbContext _context;

        public ResourcePackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResourcePack>> GetAllAsync(string? searchString = null)
        {
            var packs = _context.ResourcePacks
                .Include(r => r.ResourceCategory)
                .Include(r => r.ResourceFeature)
                .Include(r => r.ResourceResolution)
                .Include(r => r.GameVersion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                packs = packs.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            return await packs.ToListAsync();
        }

        public async Task<ResourcePack?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.ResourcePacks
                .Include(r => r.ResourceCategory)
                .Include(r => r.ResourceFeature)
                .Include(r => r.ResourceResolution)
                .Include(r => r.GameVersion)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ResourcePack?> GetByIdAsync(int id)
        {
            return await _context.ResourcePacks.FindAsync(id);
        }

        public async Task CreateAsync(ResourcePack resourcePack)
        {
            _context.ResourcePacks.Add(resourcePack);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ResourcePack resourcePack)
        {
            _context.ResourcePacks.Update(resourcePack);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pack = await _context.ResourcePacks.FindAsync(id);
            if (pack != null)
            {
                _context.ResourcePacks.Remove(pack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ResourcePacks.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<ResourceCategory>> GetCategoriesAsync()
        {
            return await _context.ResourceCategories.ToListAsync();
        }

        public async Task<IEnumerable<ResourceFeature>> GetFeaturesAsync()
        {
            return await _context.ResourceFeatures.ToListAsync();
        }

        public async Task<IEnumerable<ResourceResolution>> GetResolutionsAsync()
        {
            return await _context.ResourceResolutions.ToListAsync();
        }

        public async Task<IEnumerable<GameVersion>> GetGameVersionsAsync()
        {
            return await _context.GameVersions.ToListAsync();
        }
        public async Task<PaginatedList<ResourcePack>> GetPaginatedAsync(string? searchString, int pageIndex, int pageSize)
        {
            var packs = _context.ResourcePacks
                .Include(r => r.ResourceCategory)
                .Include(r => r.ResourceFeature)
                .Include(r => r.ResourceResolution)
                .Include(r => r.GameVersion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                packs = packs.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            packs = packs.OrderByDescending(r => r.Id);

            return await PaginatedList<ResourcePack>.CreateAsync(packs, pageIndex, pageSize);
        }
    }
}