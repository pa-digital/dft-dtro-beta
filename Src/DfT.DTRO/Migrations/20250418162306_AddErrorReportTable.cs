using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TroId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    OtherType = table.Column<string>(type: "text", nullable: true),
                    MoreInformation = table.Column<string>(type: "text", nullable: true),
                    FilePaths = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrorReport_Dtros_TroId",
                        column: x => x.TroId,
                        principalTable: "Dtros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ErrorReport_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_ErrorReport_TroId",
                table: "ErrorReport",
                column: "TroId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorReport_UserId",
                table: "ErrorReport",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorReport");

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
