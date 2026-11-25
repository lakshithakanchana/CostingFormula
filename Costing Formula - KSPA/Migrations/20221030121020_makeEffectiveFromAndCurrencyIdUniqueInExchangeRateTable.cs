using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class makeEffectiveFromAndCurrencyIdUniqueInExchangeRateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId_EffectiveFrom",
                table: "ExchangeRates",
                columns: new[] { "CurrencyId", "EffectiveFrom" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyId_EffectiveFrom",
                table: "ExchangeRates");
        }
    }
}
