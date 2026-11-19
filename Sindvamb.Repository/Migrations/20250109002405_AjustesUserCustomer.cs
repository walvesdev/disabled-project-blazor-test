using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AjustesUserCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "enabled",
                schema: "sindvamb",
                table: "customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "sindvamb",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                schema: "sindvamb",
                table: "customer",
                type: "boolean",
                nullable: true);
        }
    }
}
