using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class AddDigitalServiceProviderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DigitalServiceProviderId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DigitalServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalServiceProviders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Users",
                column: "DigitalServiceProviderId",
                principalTable: "DigitalServiceProviders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_DigitalServiceProviders_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "DigitalServiceProviders");

            migrationBuilder.DropIndex(
                name: "IX_Users_DigitalServiceProviderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DigitalServiceProviderId",
                table: "Users");
        }
    }
}
