using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RemovePrintPercentageFieldFromCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrintAreaPercentage",
                table: "CorruCostings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrintAreaPercentage",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
