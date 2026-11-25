using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddSuggestedUPrinceAndAdjustableUPriceColsToCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PerUnitAdjustableUnitPrice",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PerUnitSuggestedUnitPrice",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerUnitAdjustableUnitPrice",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "PerUnitSuggestedUnitPrice",
                table: "CorruCostings");
        }
    }
}
