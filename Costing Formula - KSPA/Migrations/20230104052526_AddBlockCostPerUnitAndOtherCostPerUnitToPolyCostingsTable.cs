using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddBlockCostPerUnitAndOtherCostPerUnitToPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BlockCostPerUnit",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OtherCostPerUnit",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockCostPerUnit",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "OtherCostPerUnit",
                table: "PolyCostings");
        }
    }
}
