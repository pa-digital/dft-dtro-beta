using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { new Guid("3153c4a8-6434-46bf-99ff-98500c02e983"), "Pending" },
                    { new Guid("6e563a53-05b6-4521-8323-896aecc27cc1"), "Active" },
                    { new Guid("c47cc91d-0d20-47b0-8fc8-ec51fb5aae94"), "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auth_UserId",
                table: "Auth",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth");

            migrationBuilder.DeleteData(
                table: "ApplicationStatus",
                keyColumn: "Id",
                keyValue: new Guid("3153c4a8-6434-46bf-99ff-98500c02e983"));

            migrationBuilder.DeleteData(
                table: "ApplicationStatus",
                keyColumn: "Id",
                keyValue: new Guid("6e563a53-05b6-4521-8323-896aecc27cc1"));

            migrationBuilder.DeleteData(
                table: "ApplicationStatus",
                keyColumn: "Id",
                keyValue: new Guid("c47cc91d-0d20-47b0-8fc8-ec51fb5aae94"));
        }
    }
}
