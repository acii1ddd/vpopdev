using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDo.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2026, 2, 23))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "toDoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_toDoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_toDoItems_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateOnly(2025, 1, 1), "alice@example.com", "Alice Johnson", "AQAAAAIAAYagAAAAEH..." },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateOnly(2025, 1, 1), "bob@example.com", "Bob Smith", "AQAAAAIAAYagAAAAEH..." },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateOnly(2025, 1, 1), "charlie@example.com", "Charlie Brown", "AQAAAAIAAYagAAAAEH..." },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateOnly(2025, 1, 1), "diana@example.com", "Diana Prince", "AQAAAAIAAYagAAAAEH..." },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateOnly(2025, 1, 1), "evan@example.com", "Evan Wright", "AQAAAAIAAYagAAAAEH..." }
                });

            migrationBuilder.InsertData(
                table: "toDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "Priority", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Молоко, хлеб, яйца", 1, "Купить продукты", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.InsertData(
                table: "toDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "IsCompleted", "Priority", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Отчет за квартал", true, 2, "Завершить отчет", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.InsertData(
                table: "toDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Тренировка ног", "Сходить в спортзал", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.InsertData(
                table: "toDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "Priority", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Главы 1-5", 1, "Прочитать книгу", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                table: "toDoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "IsCompleted", "Priority", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Обсудить детали проекта", true, 2, "Позвонить клиенту", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.CreateIndex(
                name: "IX_toDoItems_UserId",
                table: "toDoItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "toDoItems");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
