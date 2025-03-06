using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class ActionTypeAndReferenceToSourceAndProvision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProvisionActionType",
                table: "Dtros",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvisionReference",
                table: "Dtros",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceActionType",
                table: "Dtros",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceReference",
                table: "Dtros",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvisionActionType",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "ProvisionReference",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "SourceActionType",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "SourceReference",
                table: "Dtros");
        }
    }
}
