using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddBagTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Cylinders",
                newName: "IsActive");

            migrationBuilder.CreateTable(
                name: "ProductFinishingTypes",
                columns: table => new
                {
                    ProductFinTyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductFinishingTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFinishingTypes", x => x.ProductFinTyId);
                });

            migrationBuilder.CreateTable(
                name: "BagTypes",
                columns: table => new
                {
                    BagTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BagTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BagTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaToCalcSArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintByWidthOrLength = table.Column<bool>(type: "bit", nullable: false),
                    SheetSizeFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Printed = table.Column<bool>(type: "bit", nullable: false),
                    SideSeal = table.Column<bool>(type: "bit", nullable: false),
                    SideGusset = table.Column<bool>(type: "bit", nullable: false),
                    HangerAttached = table.Column<bool>(type: "bit", nullable: false),
                    Flap = table.Column<bool>(type: "bit", nullable: false),
                    HangerBag = table.Column<bool>(type: "bit", nullable: false),
                    BottomGusset = table.Column<bool>(type: "bit", nullable: false),
                    HandleCut = table.Column<bool>(type: "bit", nullable: false),
                    BottomSeal = table.Column<bool>(type: "bit", nullable: false),
                    PunchHole = table.Column<bool>(type: "bit", nullable: false),
                    TopSeal = table.Column<bool>(type: "bit", nullable: false),
                    Zipper = table.Column<bool>(type: "bit", nullable: false),
                    Isactive = table.Column<bool>(type: "bit", nullable: false),
                    ProductFinishingTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagTypes", x => x.BagTypeId);
                    table.ForeignKey(
                        name: "FK_BagTypes_ProductFinishingTypes_ProductFinishingTypeId",
                        column: x => x.ProductFinishingTypeId,
                        principalTable: "ProductFinishingTypes",
                        principalColumn: "ProductFinTyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BagTypes_ProductFinishingTypeId",
                table: "BagTypes",
                column: "ProductFinishingTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BagTypes");

            migrationBuilder.DropTable(
                name: "ProductFinishingTypes");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Cylinders",
                newName: "isActive");
        }
    }
}
