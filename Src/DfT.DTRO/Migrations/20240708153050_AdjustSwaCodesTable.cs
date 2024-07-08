using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class AdjustSwaCodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "SwaCodes",
                newName: "TraId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "SwaCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "SwaCodes");

            migrationBuilder.RenameColumn(
                name: "TraId",
                table: "SwaCodes",
                newName: "Code");
        }
    }
}
