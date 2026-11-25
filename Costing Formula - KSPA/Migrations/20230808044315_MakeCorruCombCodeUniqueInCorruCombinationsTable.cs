using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeCorruCombCodeUniqueInCorruCombinationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CorruCombCode",
                table: "CorruCombinations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CorruCombinations_CorruCombCode",
                table: "CorruCombinations",
                column: "CorruCombCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CorruCombinations_CorruCombCode",
                table: "CorruCombinations");

            migrationBuilder.AlterColumn<string>(
                name: "CorruCombCode",
                table: "CorruCombinations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
