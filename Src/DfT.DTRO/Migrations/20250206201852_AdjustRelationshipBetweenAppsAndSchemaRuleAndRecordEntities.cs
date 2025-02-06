using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AdjustRelationshipBetweenAppsAndSchemaRuleAndRecordEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_SchemaTemplate_ApplicationId",
                table: "SchemaTemplate");

            migrationBuilder.DropIndex(
                name: "IX_RuleTemplate_ApplicationId",
                table: "RuleTemplate");

            migrationBuilder.DropIndex(
                name: "IX_Dtros_ApplicationId",
                table: "Dtros");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "SchemaTemplate",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "RuleTemplate",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "Dtros",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "SchemaTemplate",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "RuleTemplate",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "Dtros",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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
    }
}
