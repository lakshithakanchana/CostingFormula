using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddBoxTypesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxTypes",
                columns: table => new
                {
                    BagTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoxTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfPlys = table.Column<int>(type: "int", nullable: false),
                    ESheetLengthFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ESheetLengthFormulaIfSLExceeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ESheetWidthFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISheetLengthFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISheetLengthFormulaIfSLExceeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISheetWidthFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Laminate = table.Column<bool>(type: "bit", nullable: false),
                    DieCut = table.Column<bool>(type: "bit", nullable: false),
                    DesignBox = table.Column<bool>(type: "bit", nullable: false),
                    Board = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxTypes", x => x.BagTypeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxTypes");
        }
    }
}
