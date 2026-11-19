using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProtocolRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "protocol",
                schema: "sindvamb",
                table: "registration_request",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "target_path",
                schema: "sindvamb",
                table: "file_control",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "sindvamb",
                table: "file_control",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "protocol",
                schema: "sindvamb",
                table: "registration_request");

            migrationBuilder.DropColumn(
                name: "description",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AlterColumn<string>(
                name: "target_path",
                schema: "sindvamb",
                table: "file_control",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
