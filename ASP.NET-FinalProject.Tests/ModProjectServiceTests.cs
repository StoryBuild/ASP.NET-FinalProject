using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ASP.NET_FinalProject.Tests
{
    [TestFixture]
    public class ModProjectServiceTests
    {
        private ApplicationDbContext _context = null!;
        private ModProjectService _service = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var category = new Category { Id = 1, Name = "Optimization" };
            var version = new GameVersion { Id = 1, VersionNumber = "1.21.4" };
            var loader = new ModLoader { Id = 1, Name = "Fabric" };

            _context.Categories.Add(category);
            _context.GameVersions.Add(version);
            _context.ModLoaders.Add(loader);

            _context.ModProjects.AddRange(
                new ModProject
                {
                    Id = 1,
                    Title = "Sodium",
                    Description = "Modern rendering engine and client-side optimization mod.",
                    Author = "jellysquid3",
                    ImageUrl = "https://example.com/sodium.png",
                    DownloadUrl = "https://example.com/download/sodium",
                    Environment = "Client",
                    Downloads = 5000,
                    CategoryId = 1,
                    GameVersionId = 1,
                    ModLoaderId = 1
                },
                new ModProject
                {
                    Id = 2,
                    Title = "Lithium",
                    Description = "General-purpose optimization mod for game physics and AI.",
                    Author = "jellysquid3",
                    ImageUrl = "https://example.com/lithium.png",
                    DownloadUrl = "https://example.com/download/lithium",
                    Environment = "Both",
                    Downloads = 3000,
                    CategoryId = 1,
                    GameVersionId = 1,
                    ModLoaderId = 1
                },
                new ModProject
                {
                    Id = 3,
                    Title = "Fabric API",
                    Description = "Lightweight and modular API for Fabric mods.",
                    Author = "modmuss50",
                    ImageUrl = "https://example.com/fabric.png",
                    DownloadUrl = "https://example.com/download/fabric-api",
                    Environment = "Both",
                    Downloads = 10000,
                    CategoryId = 1,
                    GameVersionId = 1,
                    ModLoaderId = 1
                }
            );

            await _context.SaveChangesAsync();
            _service = new ModProjectService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllMods_WhenSearchStringIsNull()
        {
            var result = (await _service.GetAllAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllAsync_FiltersByTitle_WhenSearchStringMatchesTitle()
        {
            var result = (await _service.GetAllAsync("Sodium")).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Sodium"));
        }

        [Test]
        public async Task GetAllAsync_FiltersByAuthor_WhenSearchStringMatchesAuthor()
        {
            var result = (await _service.GetAllAsync("jellysquid3")).ToList();

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
            var result = await _service.GetPaginatedAsync("Fabric", pageIndex: 1, pageSize: 6);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Fabric API"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsModWithNavigationProperties_WhenExists()
        {
            var mod = await _service.GetByIdWithDetailsAsync(1);

            Assert.That(mod, Is.Not.Null);
            Assert.That(mod!.Title, Is.EqualTo("Sodium"));
            Assert.That(mod.Category, Is.Not.Null);
            Assert.That(mod.Category.Name, Is.EqualTo("Optimization"));
            Assert.That(mod.GameVersion.VersionNumber, Is.EqualTo("1.21.4"));
            Assert.That(mod.ModLoader.Name, Is.EqualTo("Fabric"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsNull_WhenDoesNotExist()
        {
            var mod = await _service.GetByIdWithDetailsAsync(999);

            Assert.That(mod, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsMod_WhenExists()
        {
            var mod = await _service.GetByIdAsync(2);

            Assert.That(mod, Is.Not.Null);
            Assert.That(mod!.Title, Is.EqualTo("Lithium"));
        }

        [Test]
        public async Task CreateAsync_AddsModToDatabase()
        {
            var newMod = new ModProject
            {
                Title = "FerriteCore",
                Description = "Memory usage optimizations.",
                Author = "malte0811",
                DownloadUrl = "https://example.com/download/ferritecore",
                Environment = "Both",
                Downloads = 1200,
                CategoryId = 1,
                GameVersionId = 1,
                ModLoaderId = 1
            };

            await _service.CreateAsync(newMod);

            Assert.That(await _context.ModProjects.CountAsync(), Is.EqualTo(4));
            Assert.That(await _context.ModProjects.AnyAsync(m => m.Title == "FerriteCore"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_ModifiesExistingMod()
        {
            var mod = await _service.GetByIdAsync(1);
            mod!.Title = "Sodium Updated";
            mod.Downloads = 9999;

            await _service.UpdateAsync(mod);

            var updated = await _context.ModProjects.FindAsync(1);
            Assert.That(updated!.Title, Is.EqualTo("Sodium Updated"));
            Assert.That(updated.Downloads, Is.EqualTo(9999));
        }

        [Test]
        public async Task DeleteAsync_RemovesMod_WhenExists()
        {
            await _service.DeleteAsync(1);

            Assert.That(await _context.ModProjects.CountAsync(), Is.EqualTo(2));
            Assert.That(await _context.ModProjects.FindAsync(1), Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DoesNotThrow_WhenModDoesNotExist()
        {
            await _service.DeleteAsync(999);

            Assert.That(await _context.ModProjects.CountAsync(), Is.EqualTo(3));
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
            var loaders = (await _service.GetModLoadersAsync()).ToList();

            Assert.That(categories.Count, Is.EqualTo(1));
            Assert.That(versions.Count, Is.EqualTo(1));
            Assert.That(loaders.Count, Is.EqualTo(1));
        }
    }
}
