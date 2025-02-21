using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class FixApplicationPurposeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationPurposes_Applications_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationPurposes_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DigitalServiceProvider",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurposeId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Applications_PurposeId",
                table: "Applications",
                column: "PurposeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationPurposes_PurposeId",
                table: "Applications",
                column: "PurposeId",
                principalTable: "ApplicationPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationPurposes_PurposeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_PurposeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "PurposeId",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "ApplicationPurposes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPurposes_ApplicationId",
                table: "ApplicationPurposes",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationPurposes_Applications_ApplicationId",
                table: "ApplicationPurposes",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
