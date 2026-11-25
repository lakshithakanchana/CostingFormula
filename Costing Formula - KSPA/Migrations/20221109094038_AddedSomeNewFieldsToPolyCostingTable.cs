using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddedSomeNewFieldsToPolyCostingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExchangeRateId",
                table: "PolyCostings",
                newName: "OrderUomId");

            migrationBuilder.RenameColumn(
                name: "BlockId",
                table: "PolyCostings",
                newName: "MeasurementUonId");

            migrationBuilder.AddColumn<int>(
                name: "BlockAvailability",
                table: "PolyCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LengthUomId",
                table: "PolyCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "OrderQty",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SpecialInstructions",
                table: "PolyCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockAvailability",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "LengthUomId",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "OrderQty",
                table: "PolyCostings");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "PolyCostings");

            migrationBuilder.RenameColumn(
                name: "OrderUomId",
                table: "PolyCostings",
                newName: "ExchangeRateId");

            migrationBuilder.RenameColumn(
                name: "MeasurementUonId",
                table: "PolyCostings",
                newName: "BlockId");
        }
    }
}
