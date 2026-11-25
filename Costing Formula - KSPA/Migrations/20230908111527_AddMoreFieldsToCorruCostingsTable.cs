using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddMoreFieldsToCorruCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AdjSheetLength",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AdjSheetWidth",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "AreExternalMeasurements",
                table: "CorruCostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "AreaAfterAdj",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BestReelSize",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOfBoards",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalBoardReq",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSheetReq",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalSheetlength",
                table: "CorruCostings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjSheetLength",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "AdjSheetWidth",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "AreExternalMeasurements",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "AreaAfterAdj",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "BestReelSize",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "NoOfBoards",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "TotalBoardReq",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "TotalSheetReq",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "TotalSheetlength",
                table: "CorruCostings");
        }
    }
}
