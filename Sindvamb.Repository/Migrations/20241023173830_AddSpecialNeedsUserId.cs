using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialNeedsUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "special_needs_user",
                schema: "sindvamb");

            migrationBuilder.DropColumn(
                name: "code",
                schema: "sindvamb",
                table: "special_needs");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "sindvamb",
                table: "special_needs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_special_needs_user_id",
                schema: "sindvamb",
                table: "special_needs",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_special_needs_user_user_id",
                schema: "sindvamb",
                table: "special_needs",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_special_needs_user_user_id",
                schema: "sindvamb",
                table: "special_needs");

            migrationBuilder.DropIndex(
                name: "ix_special_needs_user_id",
                schema: "sindvamb",
                table: "special_needs");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "sindvamb",
                table: "special_needs");

            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "sindvamb",
                table: "special_needs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "special_needs_user",
                schema: "sindvamb",
                columns: table => new
                {
                    special_needs_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_needs_user", x => new { x.special_needs_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_special_needs_user_special_needs_special_needs_id",
                        column: x => x.special_needs_id,
                        principalSchema: "sindvamb",
                        principalTable: "special_needs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_special_needs_user_user_users_id",
                        column: x => x.users_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_special_needs_user_users_id",
                schema: "sindvamb",
                table: "special_needs_user",
                column: "users_id");
        }
    }
}
