using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class AddSwaCodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SwaCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwaCodes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwaCodes");
        }
    }
}
