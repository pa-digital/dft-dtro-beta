using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class RulesChangedToText : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "RuleTemplate");

        migrationBuilder.AlterColumn<string>(
            name: "Template",
            table: "RuleTemplate",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "jsonb");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Template",
            table: "RuleTemplate",
            type: "jsonb",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "RuleTemplate",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }
}
