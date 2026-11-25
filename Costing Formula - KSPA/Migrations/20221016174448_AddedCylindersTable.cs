using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddedCylindersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cylinders",
                columns: table => new
                {
                    CylinderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CylinderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CylinderDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfColors = table.Column<int>(type: "int", nullable: false),
                    CylinderSizeCm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfPieces = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cylinders", x => x.CylinderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cylinders");
        }
    }
}
