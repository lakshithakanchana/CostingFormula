using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddFewMoreColsToCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BlockCostTotal",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OtherCostTotal",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PreCostingWastePercentage",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockCostTotal",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "OtherCostTotal",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "PreCostingWastePercentage",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "CorruCostings");
        }
    }
}
