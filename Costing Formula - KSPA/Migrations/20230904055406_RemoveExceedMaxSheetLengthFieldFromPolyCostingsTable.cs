using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RemoveExceedMaxSheetLengthFieldFromPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceedMaxSheetLength",
                table: "CorruCostings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExceedMaxSheetLength",
                table: "CorruCostings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
