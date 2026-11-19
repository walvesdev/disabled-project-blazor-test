using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProfession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profession",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profession",
                schema: "sindvamb",
                table: "user");
        }
    }
}
