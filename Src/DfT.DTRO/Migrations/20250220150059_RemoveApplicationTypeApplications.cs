using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApplicationTypeApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTypes_Applications_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTypes_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationTypeId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationTypes_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationTypes_ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "ApplicationTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTypes_ApplicationId",
                table: "ApplicationTypes",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTypes_Applications_ApplicationId",
                table: "ApplicationTypes",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
