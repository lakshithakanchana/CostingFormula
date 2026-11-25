using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class MakeICCodeAndDescUniqueInItemCategoiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ItemCategoryDesc",
                table: "ItemCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCategoryCode",
                table: "ItemCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_ItemCategoryCode",
                table: "ItemCategories",
                column: "ItemCategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_ItemCategoryDesc",
                table: "ItemCategories",
                column: "ItemCategoryDesc",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_ItemCategoryCode",
                table: "ItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_ItemCategories_ItemCategoryDesc",
                table: "ItemCategories");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCategoryDesc",
                table: "ItemCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCategoryCode",
                table: "ItemCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
