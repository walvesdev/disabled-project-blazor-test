using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableContactIdContactRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_request_contact_contact_id",
                schema: "sindvamb",
                table: "contact_request");

            migrationBuilder.AlterColumn<Guid>(
                name: "contact_id",
                schema: "sindvamb",
                table: "contact_request",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_contact_request_contact_contact_id",
                schema: "sindvamb",
                table: "contact_request",
                column: "contact_id",
                principalSchema: "sindvamb",
                principalTable: "contact",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contact_request_contact_contact_id",
                schema: "sindvamb",
                table: "contact_request");

            migrationBuilder.AlterColumn<Guid>(
                name: "contact_id",
                schema: "sindvamb",
                table: "contact_request",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_contact_request_contact_contact_id",
                schema: "sindvamb",
                table: "contact_request",
                column: "contact_id",
                principalSchema: "sindvamb",
                principalTable: "contact",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
