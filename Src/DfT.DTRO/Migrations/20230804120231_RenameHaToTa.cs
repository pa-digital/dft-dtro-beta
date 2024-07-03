using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class RenameHaToTa : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "HA",
            table: "Dtros",
            newName: "TA");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "TA",
            table: "Dtros",
            newName: "HA");
    }
}
