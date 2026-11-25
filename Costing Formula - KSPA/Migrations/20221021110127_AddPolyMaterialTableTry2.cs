using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddPolyMaterialTableTry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolyMaterials",
                columns: table => new
                {
                    PolyMaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolyMatCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolyMaterials", x => x.PolyMaterialId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolyMaterials");
        }
    }
}
