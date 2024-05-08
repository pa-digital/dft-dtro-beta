using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Dtros",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchemaVersion = table.Column<string>(type: "text", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                Deleted = table.Column<bool>(type: "boolean", nullable: false),
                DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Data = table.Column<string>(type: "jsonb", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Dtros", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Dtros");
    }
}
