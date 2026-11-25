using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddCustomerTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PolyCombinations",
                columns: table => new
                {
                    PolyCombId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolyCombCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolyCombDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolyCombinations", x => x.PolyCombId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolyCombItemCatBridges_ItemCatId",
                table: "PolyCombItemCatBridges",
                column: "ItemCatId");

            migrationBuilder.CreateIndex(
                name: "IX_PolyCombItemCatBridges_PolyCombId",
                table: "PolyCombItemCatBridges",
                column: "PolyCombId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCombItemCatBridges_ItemCategories_ItemCatId",
                table: "PolyCombItemCatBridges",
                column: "ItemCatId",
                principalTable: "ItemCategories",
                principalColumn: "ItemCatId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolyCombItemCatBridges_PolyCombinations_PolyCombId",
                table: "PolyCombItemCatBridges",
                column: "PolyCombId",
                principalTable: "PolyCombinations",
                principalColumn: "PolyCombId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolyCombItemCatBridges_ItemCategories_ItemCatId",
                table: "PolyCombItemCatBridges");

            migrationBuilder.DropForeignKey(
                name: "FK_PolyCombItemCatBridges_PolyCombinations_PolyCombId",
                table: "PolyCombItemCatBridges");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "PolyCombinations");

            migrationBuilder.DropIndex(
                name: "IX_PolyCombItemCatBridges_ItemCatId",
                table: "PolyCombItemCatBridges");

            migrationBuilder.DropIndex(
                name: "IX_PolyCombItemCatBridges_PolyCombId",
                table: "PolyCombItemCatBridges");
        }
    }
}
