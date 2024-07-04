using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class ChangeTraToTraCreatorAndAddTraOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TA",
                table: "Dtros",
                newName: "TaOwner");

            migrationBuilder.AddColumn<int>(
                name: "TaCreator",
                table: "Dtros",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaCreator",
                table: "Dtros");

            migrationBuilder.RenameColumn(
                name: "TaOwner",
                table: "Dtros",
                newName: "TA");
        }
    }
}
