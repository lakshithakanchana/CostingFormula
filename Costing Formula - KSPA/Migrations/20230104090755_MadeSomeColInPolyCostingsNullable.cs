using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MadeSomeColInPolyCostingsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings");

            migrationBuilder.AlterColumn<double>(
                name: "Turns",
                table: "PolyCostings",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PrintTypeId",
                table: "PolyCostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CylinderId",
                table: "PolyCostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings",
                column: "CylinderId",
                principalTable: "Cylinders",
                principalColumn: "CylinderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings",
                column: "PrintTypeId",
                principalTable: "PrintTypes",
                principalColumn: "PrintTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings");

            migrationBuilder.AlterColumn<double>(
                name: "Turns",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrintTypeId",
                table: "PolyCostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CylinderId",
                table: "PolyCostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings",
                column: "CylinderId",
                principalTable: "Cylinders",
                principalColumn: "CylinderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings",
                column: "PrintTypeId",
                principalTable: "PrintTypes",
                principalColumn: "PrintTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
