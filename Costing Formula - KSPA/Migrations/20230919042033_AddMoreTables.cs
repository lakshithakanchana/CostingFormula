using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddMoreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DieCut",
                table: "CorruCostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DieCutId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DieCutTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinishingTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LaminateTypeId",
                table: "CorruCostings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Laminated",
                table: "CorruCostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DieCutTypes",
                columns: table => new
                {
                    DieCutTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieCutTypes", x => x.DieCutTypeId);
                });

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

            migrationBuilder.CreateTable(
                name: "LaminateTypes",
                columns: table => new
                {
                    LaminateTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaminateTypes", x => x.LaminateTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_DieCutTypeId",
                table: "CorruCostings",
                column: "DieCutTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_FinishingTypeId",
                table: "CorruCostings",
                column: "FinishingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCostings_LaminateTypeId",
                table: "CorruCostings",
                column: "LaminateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DieCutTypes_Desc",
                table: "DieCutTypes",
                column: "Desc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinishingTypes_Desc",
                table: "FinishingTypes",
                column: "Desc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaminateTypes_Desc",
                table: "LaminateTypes",
                column: "Desc",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_DieCutTypes_DieCutTypeId",
                table: "CorruCostings",
                column: "DieCutTypeId",
                principalTable: "DieCutTypes",
                principalColumn: "DieCutTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings",
                column: "FinishingTypeId",
                principalTable: "FinishingTypes",
                principalColumn: "FinishingTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings",
                column: "LaminateTypeId",
                principalTable: "LaminateTypes",
                principalColumn: "LaminateTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_DieCutTypes_DieCutTypeId",
                table: "CorruCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_FinishingTypes_FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.DropForeignKey(
                name: "FK_CorruCostings_LaminateTypes_LaminateTypeId",
                table: "CorruCostings");

            migrationBuilder.DropTable(
                name: "DieCutTypes");

            migrationBuilder.DropTable(
                name: "FinishingTypes");

            migrationBuilder.DropTable(
                name: "LaminateTypes");

            migrationBuilder.DropIndex(
                name: "IX_CorruCostings_DieCutTypeId",
                table: "CorruCostings");

            migrationBuilder.DropIndex(
                name: "IX_CorruCostings_FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.DropIndex(
                name: "IX_CorruCostings_LaminateTypeId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "DieCut",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "DieCutId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "DieCutTypeId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "FinishingTypeId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "LaminateTypeId",
                table: "CorruCostings");

            migrationBuilder.DropColumn(
                name: "Laminated",
                table: "CorruCostings");
        }
    }
}
