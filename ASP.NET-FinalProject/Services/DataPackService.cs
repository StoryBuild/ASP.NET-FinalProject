using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Services
{
    public class DataPackService : IDataPackService
    {
        private readonly ApplicationDbContext _context;

        public DataPackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DataPack>> GetAllAsync(string? searchString = null)
        {
            var dataPacks = _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                dataPacks = dataPacks.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            return await dataPacks.ToListAsync();
        }

        public async Task<DataPack?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<DataPack?> GetByIdAsync(int id)
        {
            return await _context.DataPacks.FindAsync(id);
        }

        public async Task CreateAsync(DataPack dataPack)
        {
            _context.DataPacks.Add(dataPack);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DataPack dataPack)
        {
            _context.DataPacks.Update(dataPack);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dataPack = await _context.DataPacks.FindAsync(id);
            if (dataPack != null)
            {
                _context.DataPacks.Remove(dataPack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DataPacks.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<DataPackCategory>> GetCategoriesAsync()
        {
            return await _context.DataPackCategories.ToListAsync();
        }

        public async Task<IEnumerable<GameVersion>> GetGameVersionsAsync()
        {
            return await _context.GameVersions.ToListAsync();
        }
        public async Task<PaginatedList<DataPack>> GetPaginatedAsync(string? searchString, int pageIndex, int pageSize)
        {
            var dataPacks = _context.DataPacks
                .Include(d => d.DataPackCategory)
                .Include(d => d.GameVersion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                dataPacks = dataPacks.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            dataPacks = dataPacks.OrderByDescending(d => d.Id);

            return await PaginatedList<DataPack>.CreateAsync(dataPacks, pageIndex, pageSize);
        }
    }
}