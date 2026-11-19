using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexCpfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "cpf_cnpj",
                schema: "sindvamb",
                table: "user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_user_cpf_cnpj",
                schema: "sindvamb",
                table: "user",
                column: "cpf_cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_cpf_cnpj",
                schema: "sindvamb",
                table: "user");

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
        }
    }
}
