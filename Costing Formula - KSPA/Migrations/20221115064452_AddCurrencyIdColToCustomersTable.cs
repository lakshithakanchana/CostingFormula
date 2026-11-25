using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddCurrencyIdColToCustomersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CurrencyId",
                table: "Customers",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Currencies_CurrencyId",
                table: "Customers",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Currencies_CurrencyId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CurrencyId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Customers");
        }
    }
}
