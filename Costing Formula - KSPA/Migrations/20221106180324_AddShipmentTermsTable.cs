using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddShipmentTermsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipmentTerms",
                columns: table => new
                {
                    ShipmentTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentTermCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipmentTermName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreightBySupplierOrCarried = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceBySupplierOrAgency = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentTerms", x => x.ShipmentTermId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentTerms");
        }
    }
}
