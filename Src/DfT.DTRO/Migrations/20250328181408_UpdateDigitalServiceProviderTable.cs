using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDigitalServiceProviderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropIndex(
                name: "IX_Applications_DigitalServiceProviderId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DigitalServiceProviderId",
                table: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "DigitalServiceProviderId",
                table: "Applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DigitalServiceProviderId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DigitalServiceProviderId",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "DigitalServiceProvider",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "DigitalServiceProvider",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DigitalServiceProvider",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DigitalServiceProviderId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                column: "DigitalServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_DigitalServiceProviderId",
                table: "Applications",
                column: "DigitalServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Applications",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProvider",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProvider",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProvider",
                principalColumn: "Id");
        }
    }
}
