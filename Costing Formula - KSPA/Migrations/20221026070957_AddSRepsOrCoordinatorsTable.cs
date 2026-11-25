using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Costing_Formula___KSPA.Migrations
{
    public partial class AddSRepsOrCoordinatorsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SRepsOrCoordinators",
                columns: table => new
                {
                    SrocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SrocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SrocContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SrocEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReprecentative = table.Column<bool>(type: "bit", nullable: false),
                    IsCoordinator = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRepsOrCoordinators", x => x.SrocId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SRepsOrCoordinators");
        }
    }
}
