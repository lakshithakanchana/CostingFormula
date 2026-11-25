using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddPolyCostingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpecialRef",
                table: "PolyCombinations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PolyCostings",
                columns: table => new
                {
                    PolyCostingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolyCostingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ExchangeRateId = table.Column<int>(type: "int", nullable: false),
                    PolyCombId = table.Column<int>(type: "int", nullable: false),
                    BagTypeId = table.Column<int>(type: "int", nullable: false),
                    Guage = table.Column<double>(type: "float", nullable: false),
                    Gusset = table.Column<double>(type: "float", nullable: false),
                    FlapWidth = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    BottomGusset = table.Column<double>(type: "float", nullable: false),
                    Printed = table.Column<bool>(type: "bit", nullable: false),
                    NoOfColors = table.Column<int>(type: "int", nullable: false),
                    NoOfPrintingSides = table.Column<int>(type: "int", nullable: false),
                    PrintTypeId = table.Column<int>(type: "int", nullable: false),
                    ShipmentTermId = table.Column<int>(type: "int", nullable: false),
                    AutoCylinderSelection = table.Column<bool>(type: "bit", nullable: false),
                    CylinderId = table.Column<int>(type: "int", nullable: false),
                    Turns = table.Column<double>(type: "float", nullable: false),
                    BlockId = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false),
                    NoOfMeters = table.Column<double>(type: "float", nullable: false),
                    PreCostingWastagePercentage = table.Column<double>(type: "float", nullable: false),
                    PreCostingWastage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolyCostings", x => x.PolyCostingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolyCostings");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialRef",
                table: "PolyCombinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
