using ASP.NET_FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ModProject> ModProjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }
        public DbSet<ModLoader> ModLoaders { get; set; }
        public DbSet<ResourcePack> ResourcePacks { get; set; }
        public DbSet<ResourceCategory> ResourceCategories { get; set; }
        public DbSet<ResourceFeature> ResourceFeatures { get; set; }
        public DbSet<ResourceResolution> ResourceResolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ModLoader>().HasData(
                new ModLoader { Id = 1, Name = "Fabric" },
                new ModLoader { Id = 2, Name = "Forge" },
                new ModLoader { Id = 3, Name = "NeoForge" }
            );

            var categories = new[] { "Adventure", "Cursed", "Decoration", "Economy", "Equipment", "Food", "Game Mechanics", "Library", "Magic", "Management", "Minigame", "Mobs", "Optimization", "Social", "Storage", "Technology", "Transportation", "Utility", "World Generation" };
            for (int i = 0; i < categories.Length; i++)
            {
                modelBuilder.Entity<Category>().HasData(new Category { Id = i + 1, Name = categories[i] });
            }

            var versions = new[] { "26.3", "26.2", "26.1.2", "26.1.1", "26.1", "1.21.11", "1.21.10", "1.21.9", "1.21.8", "1.21.7", "1.21.6", "1.21.5", "1.21.4", "1.21.3", "1.21.2", "1.21.1", "1.21", "1.20.6", "1.20.5", "1.20.4", "1.20.3", "1.20.2", "1.20.1", "1.20", "1.19.4", "1.19.3", "1.19.2", "1.19.1", "1.19", "1.18.2", "1.18.1", "1.18", "1.17.1", "1.17", "1.16.5", "1.16.4", "1.16.3", "1.16.2", "1.16.1", "1.16", "1.15.2", "1.15.1", "1.15", "1.14.4", "1.14.3", "1.14.2", "1.14.1", "1.14", "1.13.2", "1.13.1", "1.13", "1.12.2", "1.12.1", "1.12", "1.11.2", "1.11.1", "1.11", "1.10.2", "1.10.1", "1.10", "1.9.4", "1.9.3", "1.9.2", "1.9.1", "1.9", "1.8.9", "1.8.8", "1.8.7", "1.8.6", "1.8.5", "1.8.4", "1.8.3", "1.8.2", "1.8.1", "1.8", "1.7.10", "1.7.9", "1.7.8", "1.7.7", "1.7.6", "1.7.5", "1.7.4", "1.7.3", "1.7.2", "1.6.4", "1.6.2", "1.6.1", "1.5.2", "1.5.1", "1.4.7", "1.4.6", "1.4.5", "1.4.4", "1.4.2", "1.3.2", "1.3.1", "1.2.5", "1.2.4", "1.2.3", "1.2.2", "1.2.1", "1.1", "1.0" };
            for (int i = 0; i < versions.Length; i++)
            {
                modelBuilder.Entity<GameVersion>().HasData(new GameVersion { Id = i + 1, VersionNumber = versions[i] });
            }

            var resCategories = new[] { "Combat", "Cursed", "Decoration", "Modded", "Realistic", "Simplistic", "Themed", "Tweaks", "Utility", "Vanilla Like" };
            for (int i = 0; i < resCategories.Length; i++)
            {
                modelBuilder.Entity<ResourceCategory>().HasData(new ResourceCategory { Id = i + 1, Name = resCategories[i] });
            }

            var resFeatures = new[] { "Audio", "Blocks", "Core Shaders", "Entities", "Environment", "Equipment", "Fonts", "GUI", "Items", "Locale", "Models" };
            for (int i = 0; i < resFeatures.Length; i++)
            {
                modelBuilder.Entity<ResourceFeature>().HasData(new ResourceFeature { Id = i + 1, Name = resFeatures[i] });
            }

            var resResolutions = new[] { "8x or lower", "16x", "32x", "48x", "64x", "128x", "256x", "512x or higher" };
            for (int i = 0; i < resResolutions.Length; i++)
            {
                modelBuilder.Entity<ResourceResolution>().HasData(new ResourceResolution { Id = i + 1, Name = resResolutions[i] });
            }
        }
    }
}
