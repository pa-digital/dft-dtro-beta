using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class NewDatabaseStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dtros");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "SchemaTemplate",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SchemaTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "SchemaTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "RuleTemplate",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RuleTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "RuleTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DtroUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "DtroUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "DtroUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "DtroUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DtroHistories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "DtroHistories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApplicationPurposes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    ApplicationTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApplicationPurposeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationPurposes_ApplicationPurposeId",
                        column: x => x.ApplicationPurposeId,
                        principalTable: "ApplicationPurposes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationTypes_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrafficRegulationAuthorityDigitalServiceProviderStatusId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorityDigitalServiceProviders_TrafficRe~",
                        column: x => x.TrafficRegulationAuthorityDigitalServiceProviderStatusId,
                        principalTable: "TrafficRegulationAuthorityDigitalServiceProviderStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Forename = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserStatusId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsCentralServiceOperator = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserStatuses_UserStatusId",
                        column: x => x.UserStatusId,
                        principalTable: "UserStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DigitalTrafficRegulationOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    RegulationStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegulationEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TraCreator = table.Column<int>(type: "integer", nullable: false),
                    CurrentTraOwner = table.Column<int>(type: "integer", nullable: false),
                    TroName = table.Column<string>(type: "text", nullable: true),
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: false),
                    RegulationTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    VehicleTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    OrderReportingPoints = table.Column<List<string>>(type: "text[]", nullable: true),
                    RegulatedPlaceTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    Location = table.Column<NpgsqlBox>(type: "box", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalTrafficRegulationOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalTrafficRegulationOrders_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrafficRegulationAuthorityDigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorities_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorities_TrafficRegulationAuthorityDigi~",
                        column: x => x.TrafficRegulationAuthorityDigitalServiceProviderId,
                        principalTable: "TrafficRegulationAuthorityDigitalServiceProviders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrafficRegulationAuthorityDigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalServiceProviders_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DigitalServiceProviders_TrafficRegulationAuthorityDigitalSe~",
                        column: x => x.TrafficRegulationAuthorityDigitalServiceProviderId,
                        principalTable: "TrafficRegulationAuthorityDigitalServiceProviders",
                        principalColumn: "Id");
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
                name: "IX_Applications_ApplicationPurposeId",
                table: "Applications",
                column: "ApplicationPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalServiceProviders_ApplicationId",
                table: "DigitalServiceProviders",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalServiceProviders_TrafficRegulationAuthorityDigitalSe~",
                table: "DigitalServiceProviders",
                column: "TrafficRegulationAuthorityDigitalServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalServiceProviders_UserId",
                table: "DigitalServiceProviders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalTrafficRegulationOrders_ApplicationId",
                table: "DigitalTrafficRegulationOrders",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorities_ApplicationId",
                table: "TrafficRegulationAuthorities",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorities_TrafficRegulationAuthorityDigi~",
                table: "TrafficRegulationAuthorities",
                column: "TrafficRegulationAuthorityDigitalServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviders_TrafficRe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                column: "TrafficRegulationAuthorityDigitalServiceProviderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserStatusId",
                table: "Users",
                column: "UserStatusId");

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
                name: "FK_RuleTemplate_Applications_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_SchemaTemplate_Applications_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropTable(
                name: "DigitalServiceProviders");

            migrationBuilder.DropTable(
                name: "DigitalTrafficRegulationOrders");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropTable(
                name: "ApplicationPurposes");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_SchemaTemplate_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropIndex(
                name: "IX_RuleTemplate_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "DtroUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "DtroUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DtroUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DtroHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DtroHistories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DtroUsers",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Dtros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<NpgsqlBox>(type: "box", nullable: false),
                    OrderReportingPoints = table.Column<List<string>>(type: "text[]", nullable: true),
                    RegulatedPlaceTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    RegulationEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegulationStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegulationTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    SchemaVersion = table.Column<string>(type: "text", nullable: false),
                    TraCreator = table.Column<int>(type: "integer", nullable: false),
                    CurrentTraOwner = table.Column<int>(type: "integer", nullable: false),
                    TroName = table.Column<string>(type: "text", nullable: true),
                    VehicleTypes = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dtros", x => x.Id);
                });
        }
    }
}
