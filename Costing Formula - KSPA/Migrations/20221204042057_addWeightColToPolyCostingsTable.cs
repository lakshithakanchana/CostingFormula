using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class addWeightColToPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreCostingWastagePercentage",
                table: "PolyCostings",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "PreCostingWastage",
                table: "PolyCostings",
                newName: "PreCostingWastePercentage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "PolyCostings",
                newName: "PreCostingWastagePercentage");

            migrationBuilder.RenameColumn(
                name: "PreCostingWastePercentage",
                table: "PolyCostings",
                newName: "PreCostingWastage");
        }
    }
}
