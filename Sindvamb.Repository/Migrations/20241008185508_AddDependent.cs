using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddDependent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sequential",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                schema: "sindvamb",
                table: "file_control",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "dependent_id",
                schema: "sindvamb",
                table: "file_control",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "file_array",
                schema: "sindvamb",
                table: "file_control",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "sindvamb",
                table: "file_control",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dependent",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cpf_cnpj = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    cell_phone = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    is_holder = table.Column<bool>(type: "boolean", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dependent", x => x.id);
                    table.ForeignKey(
                        name: "fk_dependent_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "sindvamb",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_control_dependent_id",
                schema: "sindvamb",
                table: "file_control",
                column: "dependent_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_control_user_id",
                schema: "sindvamb",
                table: "file_control",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_dependent_customer_id",
                schema: "sindvamb",
                table: "dependent",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control",
                column: "dependent_id",
                principalSchema: "sindvamb",
                principalTable: "dependent",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropTable(
                name: "dependent",
                schema: "sindvamb");

            migrationBuilder.DropIndex(
                name: "ix_file_control_dependent_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropIndex(
                name: "ix_file_control_user_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropColumn(
                name: "content_type",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropColumn(
                name: "dependent_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropColumn(
                name: "file_array",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AddColumn<long>(
                name: "sequential",
                schema: "sindvamb",
                table: "file_control",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
