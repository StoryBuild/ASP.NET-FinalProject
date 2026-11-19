using ASP.NET_FinalProject.Models;

namespace ASP.NET_FinalProject.Services.Interfaces
{
    public interface IResourcePackService
    {
        Task<IEnumerable<ResourcePack>> GetAllAsync(string? searchString = null);
        Task<ResourcePack?> GetByIdWithDetailsAsync(int id);
        Task<ResourcePack?> GetByIdAsync(int id);
        Task CreateAsync(ResourcePack resourcePack);
        Task UpdateAsync(ResourcePack resourcePack);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<ResourceCategory>> GetCategoriesAsync();
        Task<IEnumerable<ResourceFeature>> GetFeaturesAsync();
        Task<IEnumerable<ResourceResolution>> GetResolutionsAsync();
        Task<IEnumerable<GameVersion>> GetGameVersionsAsync();
    }
}