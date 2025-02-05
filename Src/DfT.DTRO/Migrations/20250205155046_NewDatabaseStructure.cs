using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class NewDatabaseStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    TrafficRegulationAuthorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationPurposeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrafficRegulationAuthorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrafficRegulationAuthorityDigitalServiceProviderStatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Forename = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCentralServiceOperator = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalServiceProviders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "IX_DigitalServiceProviders_UserId",
                table: "DigitalServiceProviders",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "DigitalServiceProviders");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorities");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses");

            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SchemaTemplate_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropIndex(
                name: "IX_RuleTemplate_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropIndex(
                name: "IX_Dtros_ApplicationId",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Dtros");
        }
    }
}
