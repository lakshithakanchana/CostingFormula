using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddItemCategoryPricesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemCategoryPrices",
                columns: table => new
                {
                    IcpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCatId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    UnitDuty = table.Column<double>(type: "float", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategoryPrices", x => x.IcpId);
                    table.ForeignKey(
                        name: "FK_ItemCategoryPrices_ItemCategories_ItemCatId",
                        column: x => x.ItemCatId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategoryPrices_ItemCatId",
                table: "ItemCategoryPrices",
                column: "ItemCatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCategoryPrices");
        }
    }
}
