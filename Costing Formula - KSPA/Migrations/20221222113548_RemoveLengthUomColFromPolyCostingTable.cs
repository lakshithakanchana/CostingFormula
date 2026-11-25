using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RemoveLengthUomColFromPolyCostingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Uoms_LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "PolyCostingCode",
                table: "PolyCostings");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstructions",
                table: "PolyCostings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpecialInstructions",
                table: "PolyCostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LengthUomId",
                table: "PolyCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PolyCostingCode",
                table: "PolyCostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_LengthUomId",
                table: "PolyCostings",
                column: "LengthUomId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Uoms_LengthUomId",
                table: "PolyCostings",
                column: "LengthUomId",
                principalTable: "Uoms",
                principalColumn: "UomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
