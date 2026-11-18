using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classrooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classrooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repair_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "New"),
                    classroom_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    assignee_user_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repair_requests", x => x.id);
                    table.CheckConstraint("ck_repair_requests_status", "\"status\" IN ('New', 'InProgress', 'Closed', 'Cancelled')");
                    table.ForeignKey(
                        name: "FK_repair_requests_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_repair_requests_users_assignee_user_id",
                        column: x => x.assignee_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_repair_requests_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "classrooms",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 205, "Аудитория 205" },
                    { 210, "Аудитория 210" },
                    { 215, "Аудитория 215" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "login", "name", "role" },
                values: new object[,]
                {
                    { 5, "teacher", "Преподаватель", "Teacher" },
                    { 6, "dispatcher", "Диспетчер", "Dispatcher" },
                    { 7, "technician", "Техник", "Technician" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_classrooms_name",
                table: "classrooms",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_repair_requests_classroom",
                table: "repair_requests",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "idx_repair_requests_created_at",
                table: "repair_requests",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_repair_requests_status",
                table: "repair_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_repair_requests_assignee_user_id",
                table: "repair_requests",
                column: "assignee_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_repair_requests_created_by_user_id",
                table: "repair_requests",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_repair_requests_number",
                table: "repair_requests",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "repair_requests");

            migrationBuilder.DropTable(
                name: "classrooms");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
