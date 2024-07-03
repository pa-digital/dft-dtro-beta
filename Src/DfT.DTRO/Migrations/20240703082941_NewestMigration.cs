using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class NewestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DtroHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DtroId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: false),
                    TraCreator = table.Column<int>(type: "integer", nullable: false),
                    CurrentTraOwner = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtroHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dtros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegulationStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegulationEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TraCreator = table.Column<int>(type: "integer", nullable: false),
                    CurrentTraOwner = table.Column<int>(type: "integer", nullable: false),
                    TroName = table.Column<string>(type: "text", nullable: true),
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: false),
                    RegulationTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    VehicleTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    OrderReportingPoints = table.Column<List<string>>(type: "text[]", nullable: true),
                    Location = table.Column<NpgsqlBox>(type: "box", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dtros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    Template = table.Column<string>(type: "text", nullable: false)
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
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
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
                name: "DtroHistories");

            migrationBuilder.DropTable(
                name: "Dtros");

            migrationBuilder.DropTable(
                name: "RuleTemplate");

            migrationBuilder.DropTable(
                name: "SchemaTemplate");
        }
    }
}
