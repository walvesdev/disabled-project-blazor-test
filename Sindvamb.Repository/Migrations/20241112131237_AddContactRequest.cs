using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddContactRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "answered",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "answered_date",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "has_viewd",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "response",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.RenameColumn(
                name: "subject",
                schema: "sindvamb",
                table: "contact",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "profile",
                schema: "sindvamb",
                table: "contact",
                newName: "main_activity_code");

            migrationBuilder.RenameColumn(
                name: "message",
                schema: "sindvamb",
                table: "contact",
                newName: "fax");

            migrationBuilder.AddColumn<Guid>(
                name: "address_id",
                schema: "sindvamb",
                table: "contact",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cpf_cnpj",
                schema: "sindvamb",
                table: "contact",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "economic_activity",
                schema: "sindvamb",
                table: "contact",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "contact_request",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    profile = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    response = table.Column<string>(type: "text", nullable: true),
                    has_viewd = table.Column<bool>(type: "boolean", nullable: false),
                    has_answered = table.Column<bool>(type: "boolean", nullable: false),
                    has_pendding = table.Column<bool>(type: "boolean", nullable: false),
                    answered_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contact_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact_request_contact_contact_id",
                        column: x => x.contact_id,
                        principalSchema: "sindvamb",
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contact_address_id",
                schema: "sindvamb",
                table: "contact",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_contact_request_contact_id",
                schema: "sindvamb",
                table: "contact_request",
                column: "contact_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contact_address_address_id",
                schema: "sindvamb",
                table: "contact",
                column: "address_id",
                principalSchema: "sindvamb",
                principalTable: "address",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_address_address_id",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropTable(
                name: "contact_request",
                schema: "sindvamb");

            migrationBuilder.DropIndex(
                name: "ix_contact_address_id",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "address_id",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "cpf_cnpj",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "economic_activity",
                schema: "sindvamb",
                table: "contact");

            migrationBuilder.RenameColumn(
                name: "phone",
                schema: "sindvamb",
                table: "contact",
                newName: "subject");

            migrationBuilder.RenameColumn(
                name: "main_activity_code",
                schema: "sindvamb",
                table: "contact",
                newName: "profile");

            migrationBuilder.RenameColumn(
                name: "fax",
                schema: "sindvamb",
                table: "contact",
                newName: "message");

            migrationBuilder.AddColumn<bool>(
                name: "answered",
                schema: "sindvamb",
                table: "contact",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "answered_date",
                schema: "sindvamb",
                table: "contact",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "sindvamb",
                table: "contact",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "has_viewd",
                schema: "sindvamb",
                table: "contact",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "response",
                schema: "sindvamb",
                table: "contact",
                type: "text",
                nullable: true);
        }
    }
}
