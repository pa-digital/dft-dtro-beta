using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateErrorReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "RegulationTypes",
                table: "ErrorReport",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Tras",
                table: "ErrorReport",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "TroTypes",
                table: "ErrorReport",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegulationTypes",
                table: "ErrorReport");

            migrationBuilder.DropColumn(
                name: "Tras",
                table: "ErrorReport");

            migrationBuilder.DropColumn(
                name: "TroTypes",
                table: "ErrorReport");
        }
    }
}
