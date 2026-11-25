using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class addBlockCostTotalAndOtherCostTotalColsToPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherCostPerUnit",
                table: "PolyCostings",
                newName: "OtherCostTotal");

            migrationBuilder.RenameColumn(
                name: "BlockCostPerUnit",
                table: "PolyCostings",
                newName: "BlockCostTotal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherCostTotal",
                table: "PolyCostings",
                newName: "OtherCostPerUnit");

            migrationBuilder.RenameColumn(
                name: "BlockCostTotal",
                table: "PolyCostings",
                newName: "BlockCostPerUnit");
        }
    }
}
