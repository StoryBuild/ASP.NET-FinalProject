using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP.NET_FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class AddDataPacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataPackCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPackCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPacks",
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
                    DataPackCategoryId = table.Column<int>(type: "int", nullable: false),
                    GameVersionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataPacks_DataPackCategories_DataPackCategoryId",
                        column: x => x.DataPackCategoryId,
                        principalTable: "DataPackCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataPacks_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DataPackCategories",
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

            migrationBuilder.CreateIndex(
                name: "IX_DataPacks_DataPackCategoryId",
                table: "DataPacks",
                column: "DataPackCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPacks_GameVersionId",
                table: "DataPacks",
                column: "GameVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPacks");

            migrationBuilder.DropTable(
                name: "DataPackCategories");
        }
    }
}
