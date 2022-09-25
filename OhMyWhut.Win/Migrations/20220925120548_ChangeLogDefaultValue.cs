using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OhMyWhut.Win.Migrations
{
    public partial class ChangeLogDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Log",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "datetime()");
        }
    }
}
