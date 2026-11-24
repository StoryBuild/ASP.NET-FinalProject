using ASP.NET_FinalProject.Data;
using ASP.NET_FinalProject.Models;
using ASP.NET_FinalProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ASP.NET_FinalProject.Tests
{
    [TestFixture]
    public class ResourcePackServiceTests
    {
        private ApplicationDbContext _context = null!;
        private ResourcePackService _service = null!;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var category = new ResourceCategory { Id = 1, Name = "Realistic" };
            var feature = new ResourceFeature { Id = 1, Name = "Custom Models" };
            var resolution = new ResourceResolution { Id = 1, Name = "32x" };
            var version = new GameVersion { Id = 1, VersionNumber = "1.21.4" };

            _context.ResourceCategories.Add(category);
            _context.ResourceFeatures.Add(feature);
            _context.ResourceResolutions.Add(resolution);
            _context.GameVersions.Add(version);

            _context.ResourcePacks.AddRange(
                new ResourcePack
                {
                    Id = 1,
                    Title = "Faithful 32x",
                    Description = "Classic Minecraft textures doubled in resolution.",
                    Author = "FaithfulTeam",
                    ImageUrl = "https://example.com/faithful.png",
                    DownloadUrl = "https://example.com/download/faithful",
                    Downloads = 8500,
                    ResourceCategoryId = 1,
                    ResourceFeatureId = 1,
                    ResourceResolutionId = 1,
                    GameVersionId = 1
                },
                new ResourcePack
                {
                    Id = 2,
                    Title = "Fresh Animations",
                    Description = "Dynamic animated entities for vanilla Minecraft.",
                    Author = "FreshLX",
                    ImageUrl = "https://example.com/fresh.png",
                    DownloadUrl = "https://example.com/download/fresh",
                    Downloads = 6200,
                    ResourceCategoryId = 1,
                    ResourceFeatureId = 1,
                    ResourceResolutionId = 1,
                    GameVersionId = 1
                },
                new ResourcePack
                {
                    Id = 3,
                    Title = "Faithful Dark UI",
                    Description = "Dark mode user interface addon for Faithful.",
                    Author = "FaithfulTeam",
                    ImageUrl = "https://example.com/faithful-dark.png",
                    DownloadUrl = "https://example.com/download/faithful-dark",
                    Downloads = 2400,
                    ResourceCategoryId = 1,
                    ResourceFeatureId = 1,
                    ResourceResolutionId = 1,
                    GameVersionId = 1
                }
            );

            await _context.SaveChangesAsync();
            _service = new ResourcePackService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllPacks_WhenSearchStringIsNull()
        {
            var result = (await _service.GetAllAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllAsync_FiltersByTitle_WhenSearchStringMatchesTitle()
        {
            var result = (await _service.GetAllAsync("Fresh")).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Fresh Animations"));
        }

        [Test]
        public async Task GetAllAsync_FiltersByAuthor_WhenSearchStringMatchesAuthor()
        {
            var result = (await _service.GetAllAsync("FaithfulTeam")).ToList();

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
            var result = await _service.GetPaginatedAsync("Dark UI", pageIndex: 1, pageSize: 6);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Faithful Dark UI"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsPackWithNavigationProperties_WhenExists()
        {
            var pack = await _service.GetByIdWithDetailsAsync(1);

            Assert.That(pack, Is.Not.Null);
            Assert.That(pack!.Title, Is.EqualTo("Faithful 32x"));
            Assert.That(pack.ResourceCategory, Is.Not.Null);
            Assert.That(pack.ResourceCategory.Name, Is.EqualTo("Realistic"));
            Assert.That(pack.ResourceFeature.Name, Is.EqualTo("Custom Models"));
            Assert.That(pack.ResourceResolution.Name, Is.EqualTo("32x"));
            Assert.That(pack.GameVersion.VersionNumber, Is.EqualTo("1.21.4"));
        }

        [Test]
        public async Task GetByIdWithDetailsAsync_ReturnsNull_WhenDoesNotExist()
        {
            var pack = await _service.GetByIdWithDetailsAsync(999);

            Assert.That(pack, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsPack_WhenExists()
        {
            var pack = await _service.GetByIdAsync(2);

            Assert.That(pack, Is.Not.Null);
            Assert.That(pack!.Title, Is.EqualTo("Fresh Animations"));
        }

        [Test]
        public async Task CreateAsync_AddsPackToDatabase()
        {
            var newPack = new ResourcePack
            {
                Title = "Bare Bones",
                Description = "Minimalist trailer-style textures.",
                Author = "RobotPants",
                DownloadUrl = "https://example.com/download/bare-bones",
                Downloads = 4100,
                ResourceCategoryId = 1,
                ResourceFeatureId = 1,
                ResourceResolutionId = 1,
                GameVersionId = 1
            };

            await _service.CreateAsync(newPack);

            Assert.That(await _context.ResourcePacks.CountAsync(), Is.EqualTo(4));
            Assert.That(await _context.ResourcePacks.AnyAsync(p => p.Title == "Bare Bones"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_ModifiesExistingPack()
        {
            var pack = await _service.GetByIdAsync(1);
            pack!.Title = "Faithful 32x Updated";
            pack.Downloads = 12000;

            await _service.UpdateAsync(pack);

            var updated = await _context.ResourcePacks.FindAsync(1);
            Assert.That(updated!.Title, Is.EqualTo("Faithful 32x Updated"));
            Assert.That(updated.Downloads, Is.EqualTo(12000));
        }

        [Test]
        public async Task DeleteAsync_RemovesPack_WhenExists()
        {
            await _service.DeleteAsync(1);

            Assert.That(await _context.ResourcePacks.CountAsync(), Is.EqualTo(2));
            Assert.That(await _context.ResourcePacks.FindAsync(1), Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DoesNotThrow_WhenPackDoesNotExist()
        {
            await _service.DeleteAsync(999);

            Assert.That(await _context.ResourcePacks.CountAsync(), Is.EqualTo(3));
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
            var features = (await _service.GetFeaturesAsync()).ToList();
            var resolutions = (await _service.GetResolutionsAsync()).ToList();
            var versions = (await _service.GetGameVersionsAsync()).ToList();

            Assert.That(categories.Count, Is.EqualTo(1));
            Assert.That(features.Count, Is.EqualTo(1));
            Assert.That(resolutions.Count, Is.EqualTo(1));
            Assert.That(versions.Count, Is.EqualTo(1));
        }
    }
}