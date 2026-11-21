using ASP.NET_FinalProject.Models;

namespace ASP.NET_FinalProject.Services.Interfaces
{
    public interface IModProjectService
    {
        Task<IEnumerable<ModProject>> GetAllAsync(string? searchString = null);
        Task<ModProject?> GetByIdWithDetailsAsync(int id);
        Task<ModProject?> GetByIdAsync(int id);
        Task CreateAsync(ModProject modProject);
        Task UpdateAsync(ModProject modProject);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<GameVersion>> GetGameVersionsAsync();
        Task<IEnumerable<ModLoader>> GetModLoadersAsync();
        Task<PaginatedList<ModProject>> GetPaginatedAsync(string? searchString, int pageIndex, int pageSize);
    }
}