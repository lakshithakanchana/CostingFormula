using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class ChangeBagTypeIdToBoxTypeIdInBoxTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BagTypeId",
                table: "BoxTypes",
                newName: "BoxTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoxTypeId",
                table: "BoxTypes",
                newName: "BagTypeId");
        }
    }
}
