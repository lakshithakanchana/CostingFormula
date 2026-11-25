using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RenameFinishingTypeIdToCorruProductFinishingTypeIdInCPFTTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishingTypeId",
                table: "CorruProductFinishingType",
                newName: "CorruProductFinishingTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorruProductFinishingTypeId",
                table: "CorruProductFinishingType",
                newName: "FinishingTypeId");
        }
    }
}
