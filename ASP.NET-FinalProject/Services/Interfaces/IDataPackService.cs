using ASP.NET_FinalProject.Models;

namespace ASP.NET_FinalProject.Services.Interfaces
{
    public interface IDataPackService
    {
        Task<IEnumerable<DataPack>> GetAllAsync(string? searchString = null);
        Task<DataPack?> GetByIdWithDetailsAsync(int id);
        Task<DataPack?> GetByIdAsync(int id);
        Task CreateAsync(DataPack dataPack);
        Task UpdateAsync(DataPack dataPack);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<DataPackCategory>> GetCategoriesAsync();
        Task<IEnumerable<GameVersion>> GetGameVersionsAsync();
    }
}