using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDigitalServiceProviderIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DigitalServiceProviderId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProvider",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DigitalServiceProviderId",
                table: "Users");
        }
    }
}
