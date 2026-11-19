using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFileControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_file_control_file_layout_layout_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AlterColumn<Guid>(
                name: "layout_id",
                schema: "sindvamb",
                table: "file_control",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_file_layout_layout_id",
                schema: "sindvamb",
                table: "file_control",
                column: "layout_id",
                principalSchema: "sindvamb",
                principalTable: "file_layout",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_file_control_file_layout_layout_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AlterColumn<Guid>(
                name: "layout_id",
                schema: "sindvamb",
                table: "file_control",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_file_layout_layout_id",
                schema: "sindvamb",
                table: "file_control",
                column: "layout_id",
                principalSchema: "sindvamb",
                principalTable: "file_layout",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
