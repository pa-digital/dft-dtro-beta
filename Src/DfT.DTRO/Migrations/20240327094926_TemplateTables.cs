using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class TemplateTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RuleTemplate",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchemaVersion = table.Column<string>(type: "text", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                Template = table.Column<string>(type: "jsonb", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RuleTemplate", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SchemaTemplate",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchemaVersion = table.Column<string>(type: "text", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                Template = table.Column<string>(type: "jsonb", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchemaTemplate", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RuleTemplate");

        migrationBuilder.DropTable(
            name: "SchemaTemplate");
    }
}
