using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class SchemaRulesCorrelation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CreatedCorrelationId",
            table: "SchemaTemplate",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastUpdatedCorrelationId",
            table: "SchemaTemplate",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CreatedCorrelationId",
            table: "RuleTemplate",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastUpdatedCorrelationId",
            table: "RuleTemplate",
            type: "text",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedCorrelationId",
            table: "SchemaTemplate");

        migrationBuilder.DropColumn(
            name: "LastUpdatedCorrelationId",
            table: "SchemaTemplate");

        migrationBuilder.DropColumn(
            name: "CreatedCorrelationId",
            table: "RuleTemplate");

        migrationBuilder.DropColumn(
            name: "LastUpdatedCorrelationId",
            table: "RuleTemplate");
    }
}
