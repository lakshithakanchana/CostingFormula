using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class RenameFinishingTypeTableToCorruProductFinishingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.DropTable(
                name: "FinishingTypes");

            migrationBuilder.RenameColumn(
                name: "FinishingTypeId",
                table: "CorruCostings",
                newName: "CorruProductFinishingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CorruCostings_FinishingTypeId",
                table: "CorruCostings",
                newName: "IX_CorruCostings_CorruProductFinishingTypeId");

            migrationBuilder.CreateTable(
                name: "CorruProductFinishingType",
                columns: table => new
                {
                    FinishingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorruProductFinishingType", x => x.FinishingTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorruProductFinishingType_Desc",
                table: "CorruProductFinishingType",
                column: "Desc",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_CorruProductFinishingType_CorruProductFinishingTypeId",
                table: "CorruCostings",
                column: "CorruProductFinishingTypeId",
                principalTable: "CorruProductFinishingType",
                principalColumn: "FinishingTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_CorruProductFinishingType_CorruProductFinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.DropTable(
                name: "CorruProductFinishingType");

            migrationBuilder.RenameColumn(
                name: "CorruProductFinishingTypeId",
                table: "CorruCostings",
                newName: "FinishingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CorruCostings_CorruProductFinishingTypeId",
                table: "CorruCostings",
                newName: "IX_CorruCostings_FinishingTypeId");

            migrationBuilder.CreateTable(
                name: "FinishingTypes",
                columns: table => new
                {
                    FinishingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishingTypes", x => x.FinishingTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinishingTypes_Desc",
                table: "FinishingTypes",
                column: "Desc",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings",
                column: "FinishingTypeId",
                principalTable: "FinishingTypes",
                principalColumn: "FinishingTypeId");
        }
    }
}
