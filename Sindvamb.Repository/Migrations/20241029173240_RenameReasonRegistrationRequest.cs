using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameReasonRegistrationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reason",
                schema: "sindvamb",
                table: "registration_request",
                newName: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                schema: "sindvamb",
                table: "registration_request",
                newName: "reason");
        }
    }
}
