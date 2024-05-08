using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;
using System;

#nullable disable

namespace DfT.DTRO.Migrations;
public partial class OtherSearchOptimizationFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<NpgsqlBox>(
            name: "Location",
            table: "Dtros",
            type: "box",
            nullable: false,
            defaultValue: new NpgsqlTypes.NpgsqlBox(0.0, 0.0, 0.0, 0.0));

        migrationBuilder.AddColumn<DateTime>(
            name: "RegulationEnd",
            table: "Dtros",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "RegulationStart",
            table: "Dtros",
            type: "timestamp with time zone",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Location",
            table: "Dtros");

        migrationBuilder.DropColumn(
            name: "RegulationEnd",
            table: "Dtros");

        migrationBuilder.DropColumn(
            name: "RegulationStart",
            table: "Dtros");
    }
}
