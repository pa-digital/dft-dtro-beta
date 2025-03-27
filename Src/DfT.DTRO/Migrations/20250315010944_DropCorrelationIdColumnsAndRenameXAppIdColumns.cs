using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class DropCorrelationIdColumnsAndRenameXAppIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedCorrelationId",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "LastUpdatedCorrelationId",
                table: "SchemaTemplate");

            migrationBuilder.DropColumn(
                name: "CreatedCorrelationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "LastUpdatedCorrelationId",
                table: "RuleTemplate");

            migrationBuilder.DropColumn(
                name: "CreatedCorrelationId",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "LastUpdatedCorrelationId",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "CreatedCorrelationId",
                table: "Dtros");

            migrationBuilder.DropColumn(
                name: "LastUpdatedCorrelationId",
                table: "Dtros");

            migrationBuilder.RenameColumn(
                name: "xAppId",
                table: "DtroUsers",
                newName: "AppId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppId",
                table: "DtroUsers",
                newName: "xAppId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedCorrelationId",
                table: "SchemaTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedCorrelationId",
                table: "SchemaTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedCorrelationId",
                table: "RuleTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedCorrelationId",
                table: "RuleTemplate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedCorrelationId",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedCorrelationId",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedCorrelationId",
                table: "Dtros",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedCorrelationId",
                table: "Dtros",
                type: "text",
                nullable: true);
        }
    }
}
