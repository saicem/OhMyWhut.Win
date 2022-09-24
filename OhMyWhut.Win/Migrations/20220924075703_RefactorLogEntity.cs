using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OhMyWhut.Win.Migrations
{
    public partial class RefactorLogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Name",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Logs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Type",
                table: "Logs",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Type",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Logs",
                type: "TEXT",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Name",
                table: "Logs",
                column: "Name");
        }
    }
}
