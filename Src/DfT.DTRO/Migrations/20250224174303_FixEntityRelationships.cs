using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class FixEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationPurposes_Applications_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTypes_Applications_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationTypes_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationPurposes_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DigitalServiceProvider",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TrafficRegulationAuthorityId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationTypeId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PurposeId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_PurposeId",
                table: "Applications",
                column: "PurposeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId",
                table: "Applications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationPurposes_PurposeId",
                table: "Applications",
                column: "PurposeId",
                principalTable: "ApplicationPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationTypes_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                principalTable: "TrafficRegulationAuthorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationPurposes_PurposeId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationTypes_ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_PurposeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_UserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "PurposeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "ApplicationTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TrafficRegulationAuthorityId",
                table: "Applications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "ApplicationPurposes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTypes_ApplicationId",
                table: "ApplicationTypes",
                column: "ApplicationId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications",
                column: "TrafficRegulationAuthorityId",
                principalTable: "TrafficRegulationAuthorities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTypes_Applications_ApplicationId",
                table: "ApplicationTypes",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
