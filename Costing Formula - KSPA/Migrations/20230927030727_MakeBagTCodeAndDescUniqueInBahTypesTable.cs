using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeBagTCodeAndDescUniqueInBahTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BagTypeDesc",
                table: "BagTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BagTypeCode",
                table: "BagTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BagTypes_BagTypeCode",
                table: "BagTypes",
                column: "BagTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagTypes_BagTypeDesc",
                table: "BagTypes",
                column: "BagTypeDesc",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BagTypes_BagTypeCode",
                table: "BagTypes");

            migrationBuilder.DropIndex(
                name: "IX_BagTypes_BagTypeDesc",
                table: "BagTypes");

            migrationBuilder.AlterColumn<string>(
                name: "BagTypeDesc",
                table: "BagTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BagTypeCode",
                table: "BagTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
