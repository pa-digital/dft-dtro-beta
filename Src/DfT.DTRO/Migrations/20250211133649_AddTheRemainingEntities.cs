using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddTheRemainingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationPurposes_ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_DigitalServiceProviders_DigitalServiceProvider~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DigitalServiceProviders",
                table: "DigitalServiceProviders");

            migrationBuilder.DropColumn(
                name: "ApplicationPurposeId",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "DigitalServiceProviders",
                newName: "DigitalServiceProvider");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "ApplicationPurposes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DigitalServiceProvider",
                table: "DigitalServiceProvider",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrafficRegulationAuthorityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                        column: x => x.DigitalServiceProviderId,
                        principalTable: "DigitalServiceProvider",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorityDigitalServiceProviders_TrafficRe~",
                        column: x => x.TrafficRegulationAuthorityId,
                        principalTable: "TrafficRegulationAuthorities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrafficRegulationAuthorityDigitalServiceProviderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegulationAuthorityDigitalServiceProviderStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficRegulationAuthorityDigitalServiceProviderStatuses_Tr~",
                        column: x => x.TrafficRegulationAuthorityDigitalServiceProviderId,
                        principalTable: "TrafficRegulationAuthorityDigitalServiceProviders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPurposes_ApplicationId",
                table: "ApplicationPurposes",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviders_DigitalSe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                column: "DigitalServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviders_TrafficRe~",
                table: "TrafficRegulationAuthorityDigitalServiceProviders",
                column: "TrafficRegulationAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficRegulationAuthorityDigitalServiceProviderStatuses_Tr~",
                table: "TrafficRegulationAuthorityDigitalServiceProviderStatuses",
                column: "TrafficRegulationAuthorityDigitalServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationPurposes_Applications_ApplicationId",
                table: "ApplicationPurposes",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Applications",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationPurposes_Applications_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_DigitalServiceProvider_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviderStatuses");

            migrationBuilder.DropTable(
                name: "TrafficRegulationAuthorityDigitalServiceProviders");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationPurposes_ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DigitalServiceProvider",
                table: "DigitalServiceProvider");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationPurposes");

            migrationBuilder.RenameTable(
                name: "DigitalServiceProvider",
                newName: "DigitalServiceProviders");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationPurposeId",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DigitalServiceProviders",
                table: "DigitalServiceProviders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationPurposeId",
                table: "Applications",
                column: "ApplicationPurposeId");

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
                name: "FK_Users_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProviders",
                principalColumn: "Id");
        }
    }
}