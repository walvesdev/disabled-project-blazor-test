using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCustomerEntityTypeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audit_user_user_id",
                schema: "sindvamb",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropForeignKey(
                name: "fk_user_address_address_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_user_address_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_registration_request_user_id",
                schema: "sindvamb",
                table: "registration_request");

            migrationBuilder.DropIndex(
                name: "ix_customer_user_id",
                schema: "sindvamb",
                table: "customer");

            migrationBuilder.AlterColumn<string>(
                name: "cpf_cnpj",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_address_id",
                schema: "sindvamb",
                table: "user",
                column: "address_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registration_request_user_id",
                schema: "sindvamb",
                table: "registration_request",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_user_id",
                schema: "sindvamb",
                table: "customer",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_audit_user_user_id",
                schema: "sindvamb",
                table: "audit",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_address_address_id",
                schema: "sindvamb",
                table: "user",
                column: "address_id",
                principalSchema: "sindvamb",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audit_user_user_id",
                schema: "sindvamb",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.DropForeignKey(
                name: "fk_user_address_address_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_user_address_id",
                schema: "sindvamb",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_registration_request_user_id",
                schema: "sindvamb",
                table: "registration_request");

            migrationBuilder.DropIndex(
                name: "ix_customer_user_id",
                schema: "sindvamb",
                table: "customer");

            migrationBuilder.AlterColumn<string>(
                name: "cpf_cnpj",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_user_address_id",
                schema: "sindvamb",
                table: "user",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_registration_request_user_id",
                schema: "sindvamb",
                table: "registration_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_user_id",
                schema: "sindvamb",
                table: "customer",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_audit_user_user_id",
                schema: "sindvamb",
                table: "audit",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_user_user_id",
                schema: "sindvamb",
                table: "file_control",
                column: "user_id",
                principalSchema: "sindvamb",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_address_address_id",
                schema: "sindvamb",
                table: "user",
                column: "address_id",
                principalSchema: "sindvamb",
                principalTable: "address",
                principalColumn: "id");
        }
    }
}
