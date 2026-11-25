using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RemovePrefixmultiplierFromPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrefixMultiplier",
                table: "Uoms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrefixMultiplier",
                table: "Uoms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
