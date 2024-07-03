using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class AggregateSearchOptimisationFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<List<string>>(
            name: "OrderReportingPoints",
            table: "Dtros",
            type: "text[]",
            nullable: true);

        migrationBuilder.AddColumn<List<string>>(
            name: "RegulationTypes",
            table: "Dtros",
            type: "text[]",
            nullable: true);

        migrationBuilder.AddColumn<List<string>>(
            name: "VehicleTypes",
            table: "Dtros",
            type: "text[]",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "OrderReportingPoints",
            table: "Dtros");

        migrationBuilder.DropColumn(
            name: "RegulationTypes",
            table: "Dtros");

        migrationBuilder.DropColumn(
            name: "VehicleTypes",
            table: "Dtros");
    }
}
