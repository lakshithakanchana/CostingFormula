using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddCorruCombItemCatBridgesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorruCombItemCatBridges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorruCombId = table.Column<int>(type: "int", nullable: false),
                    ItemCatId = table.Column<int>(type: "int", nullable: false),
                    Gsm = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorruCombItemCatBridges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorruCombItemCatBridges_CorruCombinations_CorruCombId",
                        column: x => x.CorruCombId,
                        principalTable: "CorruCombinations",
                        principalColumn: "CorruCombId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCombItemCatBridges_ItemCategories_ItemCatId",
                        column: x => x.ItemCatId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorruCombItemCatBridges_CorruCombId",
                table: "CorruCombItemCatBridges",
                column: "CorruCombId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCombItemCatBridges_ItemCatId",
                table: "CorruCombItemCatBridges",
                column: "ItemCatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorruCombItemCatBridges");
        }
    }
}
