using Microsoft.EntityFrameworkCore.Migrations;

namespace SkiveCollegeMotion.Migrations
{
    public partial class tob1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tilmelding",
                table: "Tilmelding");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tilmelding",
                table: "Tilmelding",
                columns: new[] { "Elev", "Dag" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tilmelding",
                table: "Tilmelding");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tilmelding",
                table: "Tilmelding",
                column: "Elev");
        }
    }
}
