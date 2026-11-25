using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeBoxTypeCodeAndDescUniqueInTheBoxTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BoxTypeDesc",
                table: "BoxTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BoxTypeCode",
                table: "BoxTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypes_BoxTypeCode",
                table: "BoxTypes",
                column: "BoxTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxTypes_BoxTypeDesc",
                table: "BoxTypes",
                column: "BoxTypeDesc",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BoxTypes_BoxTypeCode",
                table: "BoxTypes");

            migrationBuilder.DropIndex(
                name: "IX_BoxTypes_BoxTypeDesc",
                table: "BoxTypes");

            migrationBuilder.AlterColumn<string>(
                name: "BoxTypeDesc",
                table: "BoxTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BoxTypeCode",
                table: "BoxTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
