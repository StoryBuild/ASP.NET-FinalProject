using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP.NET_FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class AddResourcePacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceResolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceResolutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourcePacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Downloads = table.Column<int>(type: "int", nullable: false),
                    ResourceCategoryId = table.Column<int>(type: "int", nullable: false),
                    ResourceFeatureId = table.Column<int>(type: "int", nullable: false),
                    ResourceResolutionId = table.Column<int>(type: "int", nullable: false),
                    GameVersionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourcePacks_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourcePacks_ResourceCategories_ResourceCategoryId",
                        column: x => x.ResourceCategoryId,
                        principalTable: "ResourceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourcePacks_ResourceFeatures_ResourceFeatureId",
                        column: x => x.ResourceFeatureId,
                        principalTable: "ResourceFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourcePacks_ResourceResolutions_ResourceResolutionId",
                        column: x => x.ResourceResolutionId,
                        principalTable: "ResourceResolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ResourceCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Combat" },
                    { 2, "Cursed" },
                    { 3, "Decoration" },
                    { 4, "Modded" },
                    { 5, "Realistic" },
                    { 6, "Simplistic" },
                    { 7, "Themed" },
                    { 8, "Tweaks" },
                    { 9, "Utility" },
                    { 10, "Vanilla Like" }
                });

            migrationBuilder.InsertData(
                table: "ResourceFeatures",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Audio" },
                    { 2, "Blocks" },
                    { 3, "Core Shaders" },
                    { 4, "Entities" },
                    { 5, "Environment" },
                    { 6, "Equipment" },
                    { 7, "Fonts" },
                    { 8, "GUI" },
                    { 9, "Items" },
                    { 10, "Locale" },
                    { 11, "Models" }
                });

            migrationBuilder.InsertData(
                table: "ResourceResolutions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "8x or lower" },
                    { 2, "16x" },
                    { 3, "32x" },
                    { 4, "48x" },
                    { 5, "64x" },
                    { 6, "128x" },
                    { 7, "256x" },
                    { 8, "512x or higher" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePacks_GameVersionId",
                table: "ResourcePacks",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePacks_ResourceCategoryId",
                table: "ResourcePacks",
                column: "ResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePacks_ResourceFeatureId",
                table: "ResourcePacks",
                column: "ResourceFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePacks_ResourceResolutionId",
                table: "ResourcePacks",
                column: "ResourceResolutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourcePacks");

            migrationBuilder.DropTable(
                name: "ResourceCategories");

            migrationBuilder.DropTable(
                name: "ResourceFeatures");

            migrationBuilder.DropTable(
                name: "ResourceResolutions");
        }
    }
}
