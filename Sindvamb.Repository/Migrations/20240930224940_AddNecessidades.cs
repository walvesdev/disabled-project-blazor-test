using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNecessidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "birthplace",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "business_phone",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cell_phone",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "civil_status",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "education_degree_id",
                schema: "sindvamb",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "father_name",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "has_special_needs",
                schema: "sindvamb",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "home_phone",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "mother_name",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "nationality",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "race",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "rg",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_name",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "special_needs_other",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "surname",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "uf_issuing_body",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "education_degree",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_education_degree", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "special_needs",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_needs", x => x.id);
                });

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
                name: "ix_user_education_degree_id",
                schema: "sindvamb",
                table: "user",
                column: "education_degree_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_needs_user_users_id",
                schema: "sindvamb",
                table: "special_needs_user",
                column: "users_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_education_degree_education_degree_id",
                schema: "sindvamb",
                table: "user",
                column: "education_degree_id",
                principalSchema: "sindvamb",
                principalTable: "education_degree",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_education_degree_education_degree_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropTable(
                name: "education_degree",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "special_needs_user",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "special_needs",
                schema: "sindvamb");

            migrationBuilder.DropIndex(
                name: "ix_user_education_degree_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "birthplace",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_phone",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "cell_phone",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "civil_status",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "education_degree_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "father_name",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "gender",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "has_special_needs",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "home_phone",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "mother_name",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "nationality",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "race",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "rg",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "social_name",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "special_needs_other",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "surname",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropColumn(
                name: "uf_issuing_body",
                schema: "sindvamb",
                table: "user");
        }
    }
}
