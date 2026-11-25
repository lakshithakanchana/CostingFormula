using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeLamTypeAndDiCutNotMandotaryInCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings");

            migrationBuilder.AlterColumn<int>(
                name: "LaminateTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DieCutId",
                table: "CorruCostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings",
                column: "LaminateTypeId",
                principalTable: "LaminateTypes",
                principalColumn: "LaminateTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings");

            migrationBuilder.AlterColumn<int>(
                name: "LaminateTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DieCutId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings",
                column: "LaminateTypeId",
                principalTable: "LaminateTypes",
                principalColumn: "LaminateTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
