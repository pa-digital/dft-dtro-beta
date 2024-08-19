using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class DtroUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwaCodes");

            migrationBuilder.DropColumn(
                name: "TraId",
                table: "Metrics");

            migrationBuilder.AddColumn<Guid>(
                name: "DtroUserId",
                table: "Metrics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DtroUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    xAppId = table.Column<Guid>(type: "uuid", nullable: false),
                    TraId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserGroup = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtroUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DtroUsers");

            migrationBuilder.DropColumn(
                name: "DtroUserId",
                table: "Metrics");

            migrationBuilder.AddColumn<int>(
                name: "TraId",
                table: "Metrics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SwaCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TraId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwaCodes", x => x.Id);
                });
        }
    }
}
