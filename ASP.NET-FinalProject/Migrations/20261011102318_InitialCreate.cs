using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP.NET_FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModLoaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModLoaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Environment = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Downloads = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    GameVersionId = table.Column<int>(type: "int", nullable: false),
                    ModLoaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModProjects_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModProjects_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModProjects_ModLoaders_ModLoaderId",
                        column: x => x.ModLoaderId,
                        principalTable: "ModLoaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Adventure" },
                    { 2, "Cursed" },
                    { 3, "Decoration" },
                    { 4, "Economy" },
                    { 5, "Equipment" },
                    { 6, "Food" },
                    { 7, "Game Mechanics" },
                    { 8, "Library" },
                    { 9, "Magic" },
                    { 10, "Management" },
                    { 11, "Minigame" },
                    { 12, "Mobs" },
                    { 13, "Optimization" },
                    { 14, "Social" },
                    { 15, "Storage" },
                    { 16, "Technology" },
                    { 17, "Transportation" },
                    { 18, "Utility" },
                    { 19, "World Generation" }
                });

            migrationBuilder.InsertData(
                table: "GameVersions",
                columns: new[] { "Id", "VersionNumber" },
                values: new object[,]
                {
                    { 1, "26.3" },
                    { 2, "26.2" },
                    { 3, "26.1.2" },
                    { 4, "26.1.1" },
                    { 5, "26.1" },
                    { 6, "1.21.11" },
                    { 7, "1.21.10" },
                    { 8, "1.21.9" },
                    { 9, "1.21.8" },
                    { 10, "1.21.7" },
                    { 11, "1.21.6" },
                    { 12, "1.21.5" },
                    { 13, "1.21.4" },
                    { 14, "1.21.3" },
                    { 15, "1.21.2" },
                    { 16, "1.21.1" },
                    { 17, "1.21" },
                    { 18, "1.20.6" },
                    { 19, "1.20.5" },
                    { 20, "1.20.4" },
                    { 21, "1.20.3" },
                    { 22, "1.20.2" },
                    { 23, "1.20.1" },
                    { 24, "1.20" },
                    { 25, "1.19.4" },
                    { 26, "1.19.3" },
                    { 27, "1.19.2" },
                    { 28, "1.19.1" },
                    { 29, "1.19" },
                    { 30, "1.18.2" },
                    { 31, "1.18.1" },
                    { 32, "1.18" },
                    { 33, "1.17.1" },
                    { 34, "1.17" },
                    { 35, "1.16.5" },
                    { 36, "1.16.4" },
                    { 37, "1.16.3" },
                    { 38, "1.16.2" },
                    { 39, "1.16.1" },
                    { 40, "1.16" },
                    { 41, "1.15.2" },
                    { 42, "1.15.1" },
                    { 43, "1.15" },
                    { 44, "1.14.4" },
                    { 45, "1.14.3" },
                    { 46, "1.14.2" },
                    { 47, "1.14.1" },
                    { 48, "1.14" },
                    { 49, "1.13.2" },
                    { 50, "1.13.1" },
                    { 51, "1.13" },
                    { 52, "1.12.2" },
                    { 53, "1.12.1" },
                    { 54, "1.12" },
                    { 55, "1.11.2" },
                    { 56, "1.11.1" },
                    { 57, "1.11" },
                    { 58, "1.10.2" },
                    { 59, "1.10.1" },
                    { 60, "1.10" },
                    { 61, "1.9.4" },
                    { 62, "1.9.3" },
                    { 63, "1.9.2" },
                    { 64, "1.9.1" },
                    { 65, "1.9" },
                    { 66, "1.8.9" },
                    { 67, "1.8.8" },
                    { 68, "1.8.7" },
                    { 69, "1.8.6" },
                    { 70, "1.8.5" },
                    { 71, "1.8.4" },
                    { 72, "1.8.3" },
                    { 73, "1.8.2" },
                    { 74, "1.8.1" },
                    { 75, "1.8" },
                    { 76, "1.7.10" },
                    { 77, "1.7.9" },
                    { 78, "1.7.8" },
                    { 79, "1.7.7" },
                    { 80, "1.7.6" },
                    { 81, "1.7.5" },
                    { 82, "1.7.4" },
                    { 83, "1.7.3" },
                    { 84, "1.7.2" },
                    { 85, "1.6.4" },
                    { 86, "1.6.2" },
                    { 87, "1.6.1" },
                    { 88, "1.5.2" },
                    { 89, "1.5.1" },
                    { 90, "1.4.7" },
                    { 91, "1.4.6" },
                    { 92, "1.4.5" },
                    { 93, "1.4.4" },
                    { 94, "1.4.2" },
                    { 95, "1.3.2" },
                    { 96, "1.3.1" },
                    { 97, "1.2.5" },
                    { 98, "1.2.4" },
                    { 99, "1.2.3" },
                    { 100, "1.2.2" },
                    { 101, "1.2.1" },
                    { 102, "1.1" },
                    { 103, "1.0" }
                });

            migrationBuilder.InsertData(
                table: "ModLoaders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fabric" },
                    { 2, "Forge" },
                    { 3, "NeoForge" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModProjects_CategoryId",
                table: "ModProjects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ModProjects_GameVersionId",
                table: "ModProjects",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModProjects_ModLoaderId",
                table: "ModProjects",
                column: "ModLoaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModProjects");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "GameVersions");

            migrationBuilder.DropTable(
                name: "ModLoaders");
        }
    }
}
