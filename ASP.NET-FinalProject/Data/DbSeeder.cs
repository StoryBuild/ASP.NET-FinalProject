using ASP.NET_FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedModProjectsAsync(context);
            await SeedResourcePacksAsync(context);
            await SeedDataPacksAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Administrator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            await EnsureUserAsync(userManager, "admin@mcportal.com", "Admin123!", "Administrator");
            await EnsureUserAsync(userManager, "user@mcportal.com", "User123!", "User");
        }

        private static async Task EnsureUserAsync(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }

        private static async Task SeedModProjectsAsync(ApplicationDbContext context)
        {
            if (await context.ModProjects.AnyAsync())
            {
                return;
            }

            var mods = new List<ModProject>
            {
                new()
                {
                    Title = "Sodium",
                    Description = "A modern rendering engine and client-side optimization mod for Minecraft that greatly improves frame rates and reduces micro-stutter.",
                    Author = "jellysquid3",
                    ImageUrl = "https://placehold.co/600x400/1e293b/38bdf8?text=Sodium",
                    DownloadUrl = "https://modrinth.com/mod/sodium",
                    Environment = "Client",
                    Downloads = 28500000,
                    CategoryId = 13,
                    GameVersionId = 13,
                    ModLoaderId = 1
                },
                new()
                {
                    Title = "Lithium",
                    Description = "General-purpose optimization mod designed to improve game physics, mob AI, and block ticking without changing vanilla behavior.",
                    Author = "jellysquid3",
                    ImageUrl = "https://placehold.co/600x400/1e293b/38bdf8?text=Lithium",
                    DownloadUrl = "https://modrinth.com/mod/lithium",
                    Environment = "Both",
                    Downloads = 19400000,
                    CategoryId = 13,
                    GameVersionId = 13,
                    ModLoaderId = 1
                },
                new()
                {
                    Title = "FerriteCore",
                    Description = "Significantly reduces Minecraft memory usage across both client and server environments by optimizing blockstate storage.",
                    Author = "malte0811",
                    ImageUrl = "https://placehold.co/600x400/1e293b/38bdf8?text=FerriteCore",
                    DownloadUrl = "https://modrinth.com/mod/ferrite-core",
                    Environment = "Both",
                    Downloads = 16200000,
                    CategoryId = 13,
                    GameVersionId = 13,
                    ModLoaderId = 3
                },
                new()
                {
                    Title = "Create",
                    Description = "A kinetic technology mod offering gears, belts, trains, and rotational machinery for building intricate automated factories.",
                    Author = "simibubi",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Create",
                    DownloadUrl = "https://modrinth.com/mod/create",
                    Environment = "Both",
                    Downloads = 22100000,
                    CategoryId = 16,
                    GameVersionId = 16,
                    ModLoaderId = 3
                },
                new()
                {
                    Title = "The Twilight Forest",
                    Description = "Adds a sprawling enchanted forest dimension filled with towering dungeons, unique bosses, and progression-based exploration.",
                    Author = "Benimatic",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Twilight+Forest",
                    DownloadUrl = "https://modrinth.com/mod/the-twilight-forest",
                    Environment = "Both",
                    Downloads = 14800000,
                    CategoryId = 1,
                    GameVersionId = 16,
                    ModLoaderId = 2
                },
                new()
                {
                    Title = "Iron's Spells 'n Spellbooks",
                    Description = "Introduces a full RPG magic system with collectible spellbooks, mana management, towers, and customizable spell scrolls.",
                    Author = "iron431",
                    ImageUrl = "https://placehold.co/600x400/1e293b/a855f7?text=Irons+Spells",
                    DownloadUrl = "https://modrinth.com/mod/irons-spells-n-spellbooks",
                    Environment = "Both",
                    Downloads = 7900000,
                    CategoryId = 9,
                    GameVersionId = 16,
                    ModLoaderId = 3
                },
                new()
                {
                    Title = "Litematica",
                    Description = "A modern client-side schematic mod that allows players to plan, preview, and verify complex builds layer by layer.",
                    Author = "masa",
                    ImageUrl = "https://placehold.co/600x400/1e293b/38bdf8?text=Litematica",
                    DownloadUrl = "https://modrinth.com/mod/litematica",
                    Environment = "Client",
                    Downloads = 11300000,
                    CategoryId = 18,
                    GameVersionId = 13,
                    ModLoaderId = 1
                }
            };

            await context.ModProjects.AddRangeAsync(mods);
            await context.SaveChangesAsync();
        }

        private static async Task SeedResourcePacksAsync(ApplicationDbContext context)
        {
            if (await context.ResourcePacks.AnyAsync())
            {
                return;
            }

            var resourcePacks = new List<ResourcePack>
            {
                new()
                {
                    Title = "Faithful 32x",
                    Description = "Doubles the resolution of vanilla Minecraft textures while carefully preserving the original art style.",
                    Author = "FaithfulTeam",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Faithful+32x",
                    DownloadUrl = "https://modrinth.com/resourcepack/faithful-32x",
                    Downloads = 9800000,
                    ResourceCategoryId = 10,
                    ResourceFeatureId = 2,
                    ResourceResolutionId = 3,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Fresh Animations",
                    Description = "Overhauls vanilla mob animations with expressive faces, smooth walking cycles, and lively idle behavior.",
                    Author = "FreshLX",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Fresh+Animations",
                    DownloadUrl = "https://modrinth.com/resourcepack/fresh-animations",
                    Downloads = 15200000,
                    ResourceCategoryId = 10,
                    ResourceFeatureId = 4,
                    ResourceResolutionId = 2,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Bare Bones",
                    Description = "Brings the clean, vibrant, simplified aesthetic of official Minecraft trailers directly into your game.",
                    Author = "RobotPants",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Bare+Bones",
                    DownloadUrl = "https://modrinth.com/resourcepack/bare-bones",
                    Downloads = 8400000,
                    ResourceCategoryId = 6,
                    ResourceFeatureId = 2,
                    ResourceResolutionId = 2,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Default Dark Mode",
                    Description = "Converts all inventory screens, crafting menus, and HUD containers into a sleek dark theme.",
                    Author = "nebuIr",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Dark+Mode+UI",
                    DownloadUrl = "https://modrinth.com/resourcepack/default-dark-mode",
                    Downloads = 4700000,
                    ResourceCategoryId = 8,
                    ResourceFeatureId = 8,
                    ResourceResolutionId = 2,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Patrix 64x",
                    Description = "High-detail realistic texture pack featuring rich natural surfaces, PBR maps, and custom foliage models.",
                    Author = "Patrix",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Patrix+64x",
                    DownloadUrl = "https://modrinth.com/resourcepack/patrix",
                    Downloads = 3100000,
                    ResourceCategoryId = 5,
                    ResourceFeatureId = 5,
                    ResourceResolutionId = 5,
                    GameVersionId = 16
                },
                new()
                {
                    Title = "Stay True",
                    Description = "Enhances vanilla biomes with connected textures, varied leaf colors, and subtle decorative block variations.",
                    Author = "haimcyfly",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=Stay+True",
                    DownloadUrl = "https://modrinth.com/resourcepack/stay-true",
                    Downloads = 6300000,
                    ResourceCategoryId = 3,
                    ResourceFeatureId = 5,
                    ResourceResolutionId = 2,
                    GameVersionId = 16
                },
                new()
                {
                    Title = "3D Default",
                    Description = "Adds subtle three-dimensional depth to doors, ladders, rails, tools, and decorative blocks across the world.",
                    Author = "GeForceLegend",
                    ImageUrl = "https://placehold.co/600x400/1e293b/22c55e?text=3D+Default",
                    DownloadUrl = "https://modrinth.com/resourcepack/3d-default",
                    Downloads = 2900000,
                    ResourceCategoryId = 10,
                    ResourceFeatureId = 11,
                    ResourceResolutionId = 2,
                    GameVersionId = 13
                }
            };

            await context.ResourcePacks.AddRangeAsync(resourcePacks);
            await context.SaveChangesAsync();
        }

        private static async Task SeedDataPacksAsync(ApplicationDbContext context)
        {
            if (await context.DataPacks.AnyAsync())
            {
                return;
            }

            var dataPacks = new List<DataPack>
            {
                new()
                {
                    Title = "Terralith",
                    Description = "Completely overhauls Overworld terrain generation with over 85 new biomes and dramatic canyons using vanilla blocks.",
                    Author = "StardustLabs",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Terralith",
                    DownloadUrl = "https://modrinth.com/datapack/terralith",
                    Downloads = 18600000,
                    DataPackCategoryId = 19,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Incendium",
                    Description = "Transforms the Nether into a treacherous realm with eight new biomes, custom fortresses, and challenging encounters.",
                    Author = "StardustLabs",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Incendium",
                    DownloadUrl = "https://modrinth.com/datapack/incendium",
                    Downloads = 9200000,
                    DataPackCategoryId = 19,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Nullscape",
                    Description = "Reimagines the End dimension with shattered islands, crystalline valleys, and soaring high-altitude terrain.",
                    Author = "StardustLabs",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Nullscape",
                    DownloadUrl = "https://modrinth.com/datapack/nullscape",
                    Downloads = 7400000,
                    DataPackCategoryId = 19,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Towns and Towers",
                    Description = "Expands village and pillager outpost variety with biome-specific architecture and detailed settlements.",
                    Author = "Biban_Auriu",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Towns+and+Towers",
                    DownloadUrl = "https://modrinth.com/datapack/towns-and-towers",
                    Downloads = 11500000,
                    DataPackCategoryId = 1,
                    GameVersionId = 16
                },
                new()
                {
                    Title = "Timber",
                    Description = "Allows players to fell entire trees by chopping the bottom log with an axe, speeding up early survival progression.",
                    Author = "VanillaTweaks",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Timber",
                    DownloadUrl = "https://modrinth.com/datapack/timber",
                    Downloads = 5100000,
                    DataPackCategoryId = 18,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Dungeons and Taverns",
                    Description = "Adds handcrafted taverns, revamped strongholds, and new exploration structures packed with loot.",
                    Author = "NovaWostra",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Dungeons+and+Taverns",
                    DownloadUrl = "https://modrinth.com/datapack/dungeons-and-taverns",
                    Downloads = 8900000,
                    DataPackCategoryId = 1,
                    GameVersionId = 13
                },
                new()
                {
                    Title = "Multiplayer Sleep",
                    Description = "Lets a configurable percentage of online players skip the night on multiplayer servers without requiring everyone to sleep.",
                    Author = "VanillaTweaks",
                    ImageUrl = "https://placehold.co/600x400/1e293b/f59e0b?text=Multiplayer+Sleep",
                    DownloadUrl = "https://modrinth.com/datapack/multiplayer-sleep",
                    Downloads = 6400000,
                    DataPackCategoryId = 7,
                    GameVersionId = 13
                }
            };

            await context.DataPacks.AddRangeAsync(dataPacks);
            await context.SaveChangesAsync();
        }
    }
}