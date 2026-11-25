using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorruCostings",
                columns: table => new
                {
                    CorruCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    BoxTypeId = table.Column<int>(type: "int", nullable: false),
                    CorruCombId = table.Column<int>(type: "int", nullable: false),
                    OrderUomId = table.Column<int>(type: "int", nullable: false),
                    PrintTypeId = table.Column<int>(type: "int", nullable: true),
                    ShipmentTermId = table.Column<int>(type: "int", nullable: false),
                    MeasurementUomId = table.Column<int>(type: "int", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderQty = table.Column<double>(type: "float", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Bredth = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    NoOfPlys = table.Column<int>(type: "int", nullable: false),
                    Printed = table.Column<bool>(type: "bit", nullable: false),
                    NoOfColors = table.Column<int>(type: "int", nullable: false),
                    NoOfPrintingSides = table.Column<int>(type: "int", nullable: false),
                    BlockAvailability = table.Column<int>(type: "int", nullable: false),
                    ExceedMaxSheetLength = table.Column<bool>(type: "bit", nullable: false),
                    PrintAreaPercentage = table.Column<double>(type: "float", nullable: false),
                    SheetLength = table.Column<double>(type: "float", nullable: false),
                    SheetWidth = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorruCostings", x => x.CorruCostingId);
                    table.ForeignKey(
                        name: "FK_CorruCostings_BoxTypes_BoxTypeId",
                        column: x => x.BoxTypeId,
                        principalTable: "BoxTypes",
                        principalColumn: "BoxTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_CorruCombinations_CorruCombId",
                        column: x => x.CorruCombId,
                        principalTable: "CorruCombinations",
                        principalColumn: "CorruCombId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_PrintTypes_PrintTypeId",
                        column: x => x.PrintTypeId,
                        principalTable: "PrintTypes",
                        principalColumn: "PrintTypeId");
                    table.ForeignKey(
                        name: "FK_CorruCostings_ShipmentTerms_ShipmentTermId",
                        column: x => x.ShipmentTermId,
                        principalTable: "ShipmentTerms",
                        principalColumn: "ShipmentTermId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_Uoms_MeasurementUomId",
                        column: x => x.MeasurementUomId,
                        principalTable: "Uoms",
                        principalColumn: "UomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorruCostings_Uoms_OrderUomId",
                        column: x => x.OrderUomId,
                        principalTable: "Uoms",
                        principalColumn: "UomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_BoxTypeId",
                table: "CorruCostings",
                column: "BoxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_CompanyId",
                table: "CorruCostings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_CorruCombId",
                table: "CorruCostings",
                column: "CorruCombId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_CustomerId",
                table: "CorruCostings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_MeasurementUomId",
                table: "CorruCostings",
                column: "MeasurementUomId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_OrderUomId",
                table: "CorruCostings",
                column: "OrderUomId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_PrintTypeId",
                table: "CorruCostings",
                column: "PrintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_ShipmentTermId",
                table: "CorruCostings",
                column: "ShipmentTermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorruCostings");
        }
    }
}
