using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddColorIdFieldToPolyCombinationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "PolyCombinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PolyMaterialId",
                table: "PolyCombinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PolyCombinations_ColorId",
                table: "PolyCombinations",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCombinations_PolyMaterialId",
                table: "PolyCombinations",
                column: "PolyMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCombinations_Colors_ColorId",
                table: "PolyCombinations",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCombinations_PolyMaterials_PolyMaterialId",
                table: "PolyCombinations",
                column: "PolyMaterialId",
                principalTable: "PolyMaterials",
                principalColumn: "PolyMaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCombinations_Colors_ColorId",
                table: "PolyCombinations");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCombinations_PolyMaterials_PolyMaterialId",
                table: "PolyCombinations");

            migrationBuilder.DropIndex(
                name: "IX_PolyCombinations_ColorId",
                table: "PolyCombinations");

            migrationBuilder.DropIndex(
                name: "IX_PolyCombinations_PolyMaterialId",
                table: "PolyCombinations");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "PolyCombinations");

            migrationBuilder.DropColumn(
                name: "PolyMaterialId",
                table: "PolyCombinations");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates",
                column: "CurrencyId",
                unique: true);
        }
    }
}
