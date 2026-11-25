using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddLaminateCostTotalAndDieCostTotalToCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DieCostTotal",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaminateCostTotal",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DieCostTotal",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "LaminateCostTotal",
                table: "CorruCostings");
        }
    }
}
