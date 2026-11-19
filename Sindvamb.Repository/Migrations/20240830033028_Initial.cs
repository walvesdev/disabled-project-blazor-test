using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindvamb.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sindvamb");

            migrationBuilder.CreateTable(
                name: "application_config",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_type",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "district",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    extenal_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_district", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email_history",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_success = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    template_key = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    message_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_email_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_layout",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    layout_name = table.Column<string>(type: "text", nullable: false),
                    layout_size = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_type",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    cpf_cnpj = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_area",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_area", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ticket_status",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    extenal_id = table.Column<int>(type: "integer", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "file_control",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    target_path = table.Column<string>(type: "text", nullable: false),
                    sequential = table.Column<long>(type: "bigint", nullable: false),
                    layout_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_control", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_control_file_layout_layout_id",
                        column: x => x.layout_id,
                        principalSchema: "sindvamb",
                        principalTable: "file_layout",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_unit",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    cpf_cnpj = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_unit", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_unit_partner_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "sindvamb",
                        principalTable: "partner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: true),
                    neighborhood = table.Column<string>(type: "text", nullable: true),
                    address_line1 = table.Column<string>(type: "text", nullable: true),
                    address_line2 = table.Column<string>(type: "text", nullable: true),
                    complement = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<long>(type: "bigint", nullable: true),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_address_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "sindvamb",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    sigla = table.Column<string>(type: "text", nullable: true),
                    protocol = table.Column<string>(type: "text", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: true),
                    order_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_area_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_order_status_order_status_id",
                        column: x => x.order_status_id,
                        principalSchema: "sindvamb",
                        principalTable: "order_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_order_type_order_type_id",
                        column: x => x.order_type_id,
                        principalSchema: "sindvamb",
                        principalTable: "order_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_partner_unit_partner_unit_id",
                        column: x => x.partner_unit_id,
                        principalSchema: "sindvamb",
                        principalTable: "partner_unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_product_area_product_area_id",
                        column: x => x.product_area_id,
                        principalSchema: "sindvamb",
                        principalTable: "product_area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_product_category_product_category_id",
                        column: x => x.product_category_id,
                        principalSchema: "sindvamb",
                        principalTable: "product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    matricula = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cpf_cnpj = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    is_customer = table.Column<bool>(type: "boolean", nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    securely_phrase = table.Column<string>(type: "text", nullable: true),
                    login_attemps = table.Column<int>(type: "integer", nullable: true),
                    password_policy_enabled = table.Column<bool>(type: "boolean", nullable: true),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_password_changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    password_reset_token = table.Column<Guid>(type: "uuid", nullable: true),
                    role_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_status_id = table.Column<Guid>(type: "uuid", nullable: true),
                    address_id = table.Column<Guid>(type: "uuid", nullable: true),
                    partner_unit_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_address_address_id",
                        column: x => x.address_id,
                        principalSchema: "sindvamb",
                        principalTable: "address",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_partner_unit_partner_unit_id",
                        column: x => x.partner_unit_id,
                        principalSchema: "sindvamb",
                        principalTable: "partner_unit",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "sindvamb",
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_user_status_user_status_id",
                        column: x => x.user_status_id,
                        principalSchema: "sindvamb",
                        principalTable: "user_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_file_control",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_control_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_file_control", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_file_control_file_control_file_control_id",
                        column: x => x.file_control_id,
                        principalSchema: "sindvamb",
                        principalTable: "file_control",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_file_control_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_property",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_property", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_property_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_tracking",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    track_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    history = table.Column<string>(type: "text", nullable: true),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_tracking_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    table_name = table.Column<string>(type: "text", nullable: true),
                    key_values = table.Column<string>(type: "text", nullable: true),
                    old_values = table.Column<string>(type: "text", nullable: true),
                    new_values = table.Column<string>(type: "text", nullable: true),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    partner_unit_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_customer_type_customer_type_id",
                        column: x => x.customer_type_id,
                        principalSchema: "sindvamb",
                        principalTable: "customer_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customer_partner_unit_partner_unit_id",
                        column: x => x.partner_unit_id,
                        principalSchema: "sindvamb",
                        principalTable: "partner_unit",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customer_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "login_history",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_success = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_login_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_login_history_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_history",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    old_password = table.Column<string>(type: "text", nullable: true),
                    new_password = table.Column<string>(type: "text", nullable: false),
                    security_code = table.Column<string>(type: "text", nullable: false),
                    has_changed = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_password_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_password_history_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registration_request",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    approved = table.Column<bool>(type: "boolean", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_registration_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_registration_request_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "sindvamb",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_order",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_sindvamb = table.Column<bool>(type: "boolean", nullable: false),
                    is_presidency = table.Column<bool>(type: "boolean", nullable: false),
                    is_client = table.Column<bool>(type: "boolean", nullable: false),
                    is_director = table.Column<bool>(type: "boolean", nullable: false),
                    is_manager = table.Column<bool>(type: "boolean", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "sindvamb",
                        principalTable: "customer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customer_order_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ticket",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    solution = table.Column<string>(type: "text", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_status_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "sindvamb",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ticket_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ticket_ticket_status_ticket_status_id",
                        column: x => x.ticket_status_id,
                        principalSchema: "sindvamb",
                        principalTable: "ticket_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "attachment",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    size = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    absolute_url = table.Column<string>(type: "text", nullable: false),
                    in_cloud = table.Column<bool>(type: "boolean", nullable: false),
                    file_data = table.Column<byte[]>(type: "bytea", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment", x => x.id);
                    table.ForeignKey(
                        name: "fk_attachment_ticket_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "sindvamb",
                        principalTable: "ticket",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_email",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_email", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_email_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "sindvamb",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_email_ticket_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "sindvamb",
                        principalTable: "ticket",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ticket_property",
                schema: "sindvamb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_property", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_property_ticket_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "sindvamb",
                        principalTable: "ticket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_address_city_id",
                schema: "sindvamb",
                table: "address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_attachment_ticket_id",
                schema: "sindvamb",
                table: "attachment",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_user_id",
                schema: "sindvamb",
                table: "audit",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_city_district_id",
                schema: "sindvamb",
                table: "city",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_customer_type_id",
                schema: "sindvamb",
                table: "customer",
                column: "customer_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_partner_unit_id",
                schema: "sindvamb",
                table: "customer",
                column: "partner_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_user_id",
                schema: "sindvamb",
                table: "customer",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_order_customer_id",
                schema: "sindvamb",
                table: "customer_order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_order_order_id",
                schema: "sindvamb",
                table: "customer_order",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_type_code",
                schema: "sindvamb",
                table: "customer_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_file_control_layout_id",
                schema: "sindvamb",
                table: "file_control",
                column: "layout_id");

            migrationBuilder.CreateIndex(
                name: "ix_login_history_user_id",
                schema: "sindvamb",
                table: "login_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_order_status_id",
                schema: "sindvamb",
                table: "order",
                column: "order_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_order_type_id",
                schema: "sindvamb",
                table: "order",
                column: "order_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_partner_unit_id",
                schema: "sindvamb",
                table: "order",
                column: "partner_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_product_area_id",
                schema: "sindvamb",
                table: "order",
                column: "product_area_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_product_category_id",
                schema: "sindvamb",
                table: "order",
                column: "product_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_email_order_id",
                schema: "sindvamb",
                table: "order_email",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_email_ticket_id",
                schema: "sindvamb",
                table: "order_email",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_file_control_file_control_id",
                schema: "sindvamb",
                table: "order_file_control",
                column: "file_control_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_file_control_order_id",
                schema: "sindvamb",
                table: "order_file_control",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_property_order_id",
                schema: "sindvamb",
                table: "order_property",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_code",
                schema: "sindvamb",
                table: "order_status",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_tracking_order_id",
                schema: "sindvamb",
                table: "order_tracking",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_type_code",
                schema: "sindvamb",
                table: "order_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_unit_partner_id",
                schema: "sindvamb",
                table: "partner_unit",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_password_history_user_id",
                schema: "sindvamb",
                table: "password_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_area_code",
                schema: "sindvamb",
                table: "product_area",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_category_code",
                schema: "sindvamb",
                table: "product_category",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registration_request_user_id",
                schema: "sindvamb",
                table: "registration_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_customer_id",
                schema: "sindvamb",
                table: "ticket",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_order_id",
                schema: "sindvamb",
                table: "ticket",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_ticket_status_id",
                schema: "sindvamb",
                table: "ticket",
                column: "ticket_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_property_ticket_id",
                schema: "sindvamb",
                table: "ticket_property",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_status_code",
                schema: "sindvamb",
                table: "ticket_status",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_address_id",
                schema: "sindvamb",
                table: "user",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_partner_unit_id",
                schema: "sindvamb",
                table: "user",
                column: "partner_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_id",
                schema: "sindvamb",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_user_status_id",
                schema: "sindvamb",
                table: "user",
                column: "user_status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_config",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "attachment",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "audit",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "customer_order",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "email_history",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "login_history",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_email",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_file_control",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_property",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_tracking",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "password_history",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "registration_request",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "ticket_property",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "file_control",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "ticket",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "file_layout",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "customer",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "ticket_status",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "customer_type",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "user",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_status",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "order_type",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "product_area",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "product_category",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "address",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "partner_unit",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "role",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "user_status",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "city",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "partner",
                schema: "sindvamb");

            migrationBuilder.DropTable(
                name: "district",
                schema: "sindvamb");
        }
    }
}
