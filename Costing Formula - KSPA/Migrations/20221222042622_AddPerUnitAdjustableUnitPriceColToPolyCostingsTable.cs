using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddPerUnitAdjustableUnitPriceColToPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PerUnitAdjustableUnitPrice",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_BagTypeId",
                table: "PolyCostings",
                column: "BagTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_CompanyId",
                table: "PolyCostings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_CustomerId",
                table: "PolyCostings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_CylinderId",
                table: "PolyCostings",
                column: "CylinderId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_LengthUomId",
                table: "PolyCostings",
                column: "LengthUomId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_MeasurementUomId",
                table: "PolyCostings",
                column: "MeasurementUomId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_OrderUomId",
                table: "PolyCostings",
                column: "OrderUomId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_PolyCombId",
                table: "PolyCostings",
                column: "PolyCombId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_PrintTypeId",
                table: "PolyCostings",
                column: "PrintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCostings_ShipmentTermId",
                table: "PolyCostings",
                column: "ShipmentTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_BagTypes_BagTypeId",
                table: "PolyCostings",
                column: "BagTypeId",
                principalTable: "BagTypes",
                principalColumn: "BagTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Companies_CompanyId",
                table: "PolyCostings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Customers_CustomerId",
                table: "PolyCostings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings",
                column: "CylinderId",
                principalTable: "Cylinders",
                principalColumn: "CylinderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_PolyCombinations_PolyCombId",
                table: "PolyCostings",
                column: "PolyCombId",
                principalTable: "PolyCombinations",
                principalColumn: "PolyCombId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings",
                column: "PrintTypeId",
                principalTable: "PrintTypes",
                principalColumn: "PrintTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_ShipmentTerms_ShipmentTermId",
                table: "PolyCostings",
                column: "ShipmentTermId",
                principalTable: "ShipmentTerms",
                principalColumn: "ShipmentTermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Uoms_LengthUomId",
                table: "PolyCostings",
                column: "LengthUomId",
                principalTable: "Uoms",
                principalColumn: "UomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Uoms_MeasurementUomId",
                table: "PolyCostings",
                column: "MeasurementUomId",
                principalTable: "Uoms",
                principalColumn: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCostings_Uoms_OrderUomId",
                table: "PolyCostings",
                column: "OrderUomId",
                principalTable: "Uoms",
                principalColumn: "UomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_BagTypes_BagTypeId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Companies_CompanyId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Customers_CustomerId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Cylinders_CylinderId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_PolyCombinations_PolyCombId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_PrintTypes_PrintTypeId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_ShipmentTerms_ShipmentTermId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Uoms_LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Uoms_MeasurementUomId",
                table: "PolyCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCostings_Uoms_OrderUomId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_BagTypeId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_CompanyId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_CustomerId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_CylinderId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_MeasurementUomId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_OrderUomId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_PolyCombId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_PrintTypeId",
                table: "PolyCostings");

            migrationBuilder.DropIndex(
                name: "IX_PolyCostings_ShipmentTermId",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "PerUnitAdjustableUnitPrice",
                table: "PolyCostings");
        }
    }
}
