using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFileLayoutEntityTypeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control",
                column: "dependent_id",
                principalSchema: "sindvamb",
                principalTable: "dependent",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control");

            migrationBuilder.AddForeignKey(
                name: "fk_file_control_dependent_dependent_id",
                schema: "sindvamb",
                table: "file_control",
                column: "dependent_id",
                principalSchema: "sindvamb",
                principalTable: "dependent",
                principalColumn: "id");
        }
    }
}
