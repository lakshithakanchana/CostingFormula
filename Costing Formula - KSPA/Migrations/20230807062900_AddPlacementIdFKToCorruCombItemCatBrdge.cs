using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddPlacementIdFKToCorruCombItemCatBrdge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlacementId",
                table: "CorruCombItemCatBridges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CorruCombItemCatBridges_PlacementId",
                table: "CorruCombItemCatBridges",
                column: "PlacementId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCombItemCatBridges_Placements_PlacementId",
                table: "CorruCombItemCatBridges",
                column: "PlacementId",
                principalTable: "Placements",
                principalColumn: "PlacementId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCombItemCatBridges_Placements_PlacementId",
                table: "CorruCombItemCatBridges");

            migrationBuilder.DropIndex(
                name: "IX_CorruCombItemCatBridges_PlacementId",
                table: "CorruCombItemCatBridges");

            migrationBuilder.DropColumn(
                name: "PlacementId",
                table: "CorruCombItemCatBridges");
        }
    }
}
