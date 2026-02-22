using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.API.Migrations
{
    /// <inheritdoc />
    public partial class FixPriorityDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2026, 2, 22),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2026, 2, 21));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2026, 2, 21),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2026, 2, 22));
        }
    }
}
