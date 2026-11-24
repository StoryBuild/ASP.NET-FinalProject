using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ASP.NET_FinalProject.Tests
{
    [TestFixture]
    public class DataPackServiceTests
    {
        private ApplicationDbContext _context = null!;
        private DataPackService _service = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var category = new DataPackCategory { Id = 1, Name = "World Generation" };
            var version = new GameVersion { Id = 1, VersionNumber = "1.21.4" };

            _context.DataPackCategories.Add(category);
            _context.GameVersions.Add(version);

            _context.DataPacks.AddRange(
                new DataPack
                {
                    Id = 1,
                    Title = "Terralith",
                    Description = "Overhauls the Overworld with over 85 new biomes.",
                    Author = "StardustLabs",
                    ImageUrl = "https://example.com/terralith.png",
                    DownloadUrl = "https://example.com/download/terralith",
                    Downloads = 9500,
                    DataPackCategoryId = 1,
                    GameVersionId = 1
                },
                new DataPack
                {
                    Id = 2,
                    Title = "Incendium",
                    Description = "Transforms the Nether with new biomes and structures.",
                    Author = "StardustLabs",
                    ImageUrl = "https://example.com/incendium.png",
                    DownloadUrl = "https://example.com/download/incendium",
                    Downloads = 6100,
                    DataPackCategoryId = 1,
                    GameVersionId = 1
                },
                new DataPack
                {
                    Id = 3,
                    Title = "Vanilla Tweaks",
                    Description = "Quality of life tweaks for survival gameplay.",
                    Author = "Xisumavoid",
                    ImageUrl = "https://example.com/vanillatweaks.png",
                    DownloadUrl = "https://example.com/download/vanillatweaks",
                    Downloads = 12000,
                    DataPackCategoryId = 1,
                    GameVersionId = 1
                }
            );

            await _context.SaveChangesAsync();
            _service = new DataPackService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllDataPacks_WhenSearchStringIsNull()
        {
            var result = (await _service.GetAllAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllAsync_FiltersByTitle_WhenSearchStringMatchesTitle()
        {
            var result = (await _service.GetAllAsync("Terralith")).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Terralith"));
        }

        [Test]
        public async Task GetAllAsync_FiltersByAuthor_WhenSearchStringMatchesAuthor()
        {
            var result = (await _service.GetAllAsync("StardustLabs")).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPaginatedAsync_ReturnsCorrectPageInDescendingOrder()
        {
            var page1 = await _service.GetPaginatedAsync(null, pageIndex: 1, pageSize: 2);
            var page2 = await _service.GetPaginatedAsync(null, pageIndex: 2, pageSize: 2);

            Assert.That(page1.Count, Is.EqualTo(2));
            Assert.That(page1.TotalPages, Is.EqualTo(2));
            Assert.That(page1[0].Id, Is.EqualTo(3));
            Assert.That(page2.Count, Is.EqualTo(1));
            Assert.That(page2[0].Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPaginatedAsync_FiltersResults_WhenSearchStringIsProvided()
        {
            var result = await _service.GetPaginatedAsync("Vanilla", pageIndex: 1, pageSize: 6);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Vanilla Tweaks"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsDataPackWithNavigationProperties_WhenExists()
        {
            var dataPack = await _service.GetByIdWithDetailsAsync(1);

            Assert.That(dataPack, Is.Not.Null);
            Assert.That(dataPack!.Title, Is.EqualTo("Terralith"));
            Assert.That(dataPack.DataPackCategory, Is.Not.Null);
            Assert.That(dataPack.DataPackCategory.Name, Is.EqualTo("World Generation"));
            Assert.That(dataPack.GameVersion.VersionNumber, Is.EqualTo("1.21.4"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsNull_WhenDoesNotExist()
        {
            var dataPack = await _service.GetByIdWithDetailsAsync(999);

            Assert.That(dataPack, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsDataPack_WhenExists()
        {
            var dataPack = await _service.GetByIdAsync(2);

            Assert.That(dataPack, Is.Not.Null);
            Assert.That(dataPack!.Title, Is.EqualTo("Incendium"));
        }

        [Test]
        public async Task CreateAsync_AddsDataPackToDatabase()
        {
            var newDataPack = new DataPack
            {
                Title = "Nullscape",
                Description = "Overhauls the End dimension terrain.",
                Author = "StardustLabs",
                DownloadUrl = "https://example.com/download/nullscape",
                Downloads = 3400,
                DataPackCategoryId = 1,
                GameVersionId = 1
            };

            await _service.CreateAsync(newDataPack);

            Assert.That(await _context.DataPacks.CountAsync(), Is.EqualTo(4));
            Assert.That(await _context.DataPacks.AnyAsync(d => d.Title == "Nullscape"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_ModifiesExistingDataPack()
        {
            var dataPack = await _service.GetByIdAsync(1);
            dataPack!.Title = "Terralith Updated";
            dataPack.Downloads = 15000;

            await _service.UpdateAsync(dataPack);

            var updated = await _context.DataPacks.FindAsync(1);
            Assert.That(updated!.Title, Is.EqualTo("Terralith Updated"));
            Assert.That(updated.Downloads, Is.EqualTo(15000));
        }

        [Test]
        public async Task DeleteAsync_RemovesDataPack_WhenExists()
        {
            await _service.DeleteAsync(1);

            Assert.That(await _context.DataPacks.CountAsync(), Is.EqualTo(2));
            Assert.That(await _context.DataPacks.FindAsync(1), Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DoesNotThrow_WhenDataPackDoesNotExist()
        {
            await _service.DeleteAsync(999);

            Assert.That(await _context.DataPacks.CountAsync(), Is.EqualTo(3));
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrue_WhenExists_AndFalse_WhenNot()
        {
            Assert.That(await _service.ExistsAsync(1), Is.True);
            Assert.That(await _service.ExistsAsync(999), Is.False);
        }

        [Test]
        public async Task GetLookupMethods_ReturnSeededEntities()
        {
            var categories = (await _service.GetCategoriesAsync()).ToList();
            var versions = (await _service.GetGameVersionsAsync()).ToList();

            Assert.That(categories.Count, Is.EqualTo(1));
            Assert.That(versions.Count, Is.EqualTo(1));
        }
    }
}