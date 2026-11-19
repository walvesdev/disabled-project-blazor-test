using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovedFileControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "approved",
                schema: "sindvamb",
                table: "file_control",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approved",
                schema: "sindvamb",
                table: "file_control");
        }
    }
}
