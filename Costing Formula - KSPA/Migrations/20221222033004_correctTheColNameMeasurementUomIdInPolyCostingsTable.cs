using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class correctTheColNameMeasurementUomIdInPolyCostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MeasurementUonId",
                table: "PolyCostings",
                newName: "MeasurementUomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MeasurementUomId",
                table: "PolyCostings",
                newName: "MeasurementUonId");
        }
    }
}
