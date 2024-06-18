using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class ChangeTaToTra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaOwner",
                table: "Dtros",
                newName: "CurrentTraOwner");

            migrationBuilder.RenameColumn(
                name: "TaCreator",
                table: "Dtros",
                newName: "TraCreator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TraCreator",
                table: "Dtros",
                newName: "TaCreator");

            migrationBuilder.RenameColumn(
                name: "CurrentTraOwner",
                table: "Dtros",
                newName: "TaOwner");
        }
    }
}
