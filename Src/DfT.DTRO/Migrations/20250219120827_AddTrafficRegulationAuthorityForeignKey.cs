using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddTrafficRegulationAuthorityForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications",
                column: "TrafficRegulationAuthorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                unique: true);
        }
    }
}
