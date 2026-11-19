using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemovedStatusRegistrationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "sindvamb",
                table: "registration_request");

            migrationBuilder.AlterColumn<bool>(
                name: "approved",
                schema: "sindvamb",
                table: "registration_request",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "approved",
                schema: "sindvamb",
                table: "registration_request",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                schema: "sindvamb",
                table: "registration_request",
                type: "text",
                nullable: true);
        }
    }
}
