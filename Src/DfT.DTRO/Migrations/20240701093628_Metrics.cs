using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class Metrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TraId = table.Column<int>(type: "integer", nullable: false),
                    ForDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SystemFailureCount = table.Column<int>(type: "integer", nullable: false),
                    SubmissionFailureCount = table.Column<int>(type: "integer", nullable: false),
                    SubmissionCount = table.Column<int>(type: "integer", nullable: false),
                    DeletionCount = table.Column<int>(type: "integer", nullable: false),
                    SearchCount = table.Column<int>(type: "integer", nullable: false),
                    EventCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedCorrelationId = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedCorrelationId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
