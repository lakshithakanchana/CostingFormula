using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeFinishingTypeNullableInCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.AlterColumn<int>(
                name: "FinishingTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings",
                column: "FinishingTypeId",
                principalTable: "FinishingTypes",
                principalColumn: "FinishingTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.AlterColumn<int>(
                name: "FinishingTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings",
                column: "FinishingTypeId",
                principalTable: "FinishingTypes",
                principalColumn: "FinishingTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
