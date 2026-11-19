using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_control",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    connection_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_beat_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    dur = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    os = table.Column<string>(type: "text", nullable: true),
                    device = table.Column<string>(type: "text", nullable: true),
                    browser = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true),
                    engine = table.Column<string>(type: "text", nullable: true),
                    request_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_access_control", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_control",
                schema: "sindvamb");
        }
    }
}
