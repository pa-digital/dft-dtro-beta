using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTypes_Application_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Application",
                table: "Application");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_Application_DigitalServiceProviderId",
                table: "Applications",
                newName: "IX_Applications_DigitalServiceProviderId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "SchemaTemplate",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "RuleTemplate",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "Dtros",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationPurposeId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrafficRegulationAuthorityId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApplicationPurposes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchemaTemplate_ApplicationId",
                table: "SchemaTemplate",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleTemplate_ApplicationId",
                table: "RuleTemplate",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Dtros_ApplicationId",
                table: "Dtros",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationPurposeId",
                table: "Applications",
                column: "ApplicationPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications",
                column: "TrafficRegulationAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationPurposes_ApplicationPurposeId",
                table: "Applications",
                column: "ApplicationPurposeId",
                principalTable: "ApplicationPurposes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_DigitalServiceProviders_DigitalServiceProvider~",
                table: "Applications",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProviders",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Dtros_Applications_ApplicationId",
                table: "Dtros",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleTemplate_Applications_ApplicationId",
                table: "RuleTemplate",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaTemplate_Applications_ApplicationId",
                table: "SchemaTemplate",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationPurposes_ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_DigitalServiceProviders_DigitalServiceProvider~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_TrafficRegulationAuthorities_TrafficRegulation~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTypes_Applications_ApplicationId",
                table: "ApplicationTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dtros_Applications_ApplicationId",
                table: "Dtros");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleTemplate_Applications_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_SchemaTemplate_Applications_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropTable(
                name: "ApplicationPurposes");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorities");

            migrationBuilder.DropIndex(
                name: "IX_SchemaTemplate_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropIndex(
                name: "IX_RuleTemplate_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropIndex(
                name: "IX_Dtros_ApplicationId",
                table: "Dtros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "TrafficRegulationAuthorityId",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_DigitalServiceProviderId",
                table: "Application",
                newName: "IX_Application_DigitalServiceProviderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Application",
                table: "Application",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Application",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProviders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTypes_Application_ApplicationId",
                table: "ApplicationTypes",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id");
        }
    }
}
