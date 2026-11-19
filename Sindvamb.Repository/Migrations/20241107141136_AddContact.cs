using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cell_phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    profile = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    response = table.Column<string>(type: "text", nullable: true),
                    has_viewd = table.Column<bool>(type: "boolean", nullable: false),
                    answered = table.Column<bool>(type: "boolean", nullable: false),
                    answered_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contact",
                schema: "sindvamb");
        }
    }
}
