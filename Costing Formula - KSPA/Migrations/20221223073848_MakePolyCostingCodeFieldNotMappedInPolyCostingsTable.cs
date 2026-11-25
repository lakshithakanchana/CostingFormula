using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakePolyCostingCodeFieldNotMappedInPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolyCostingCode",
                table: "PolyCostings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PolyCostingCode",
                table: "PolyCostings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
