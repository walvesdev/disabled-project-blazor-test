using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_address_city_city_id",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.DropTable(
                name: "city",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "district",
                schema: "sindvamb");

            migrationBuilder.DropIndex(
                name: "ix_address_city_id",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.DropColumn(
                name: "city_id",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.AddColumn<string>(
                name: "city",
                schema: "sindvamb",
                table: "address",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "district",
                schema: "sindvamb",
                table: "address",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "housing",
                schema: "sindvamb",
                table: "address",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uf",
                schema: "sindvamb",
                table: "address",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.DropColumn(
                name: "district",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.DropColumn(
                name: "housing",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.DropColumn(
                name: "uf",
                schema: "sindvamb",
                table: "address");

            migrationBuilder.AddColumn<Guid>(
                name: "city_id",
                schema: "sindvamb",
                table: "address",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "district",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    extenal_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_district", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    extenal_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_city", x => x.id);
                    table.ForeignKey(
                        name: "fk_city_district_district_id",
                        column: x => x.district_id,
                        principalSchema: "sindvamb",
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_address_city_id",
                schema: "sindvamb",
                table: "address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_city_district_id",
                schema: "sindvamb",
                table: "city",
                column: "district_id");

            migrationBuilder.AddForeignKey(
                name: "fk_address_city_city_id",
                schema: "sindvamb",
                table: "address",
                column: "city_id",
                principalSchema: "sindvamb",
                principalTable: "city",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
