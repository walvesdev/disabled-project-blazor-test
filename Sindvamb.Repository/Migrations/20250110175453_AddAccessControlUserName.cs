using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessControlUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "sindvamb",
                table: "access_control",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_name",
                schema: "sindvamb",
                table: "access_control",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "sindvamb",
                table: "access_control");

            migrationBuilder.DropColumn(
                name: "user_name",
                schema: "sindvamb",
                table: "access_control");
        }
    }
}
