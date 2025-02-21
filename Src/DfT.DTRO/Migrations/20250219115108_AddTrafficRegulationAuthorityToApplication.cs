using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddTrafficRegulationAuthorityToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.AlterColumn<Guid>(
                name: "TrafficRegulationAuthorityId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                principalTable: "TrafficRegulationAuthorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.AlterColumn<Guid>(
                name: "TrafficRegulationAuthorityId",
                table: "Applications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications",
                column: "TrafficRegulationAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                principalTable: "TrafficRegulationAuthorities",
                principalColumn: "Id");
        }
    }
}
