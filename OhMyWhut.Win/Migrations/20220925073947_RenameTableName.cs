using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OhMyWhut.Win.Migrations
{
    public partial class RenameTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyCourses",
                table: "MyCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricFees",
                table: "ElectricFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllCourses",
                table: "AllCourses");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "Preference");

            migrationBuilder.RenameTable(
                name: "MyCourses",
                newName: "MyCourse");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "Log");

            migrationBuilder.RenameTable(
                name: "ElectricFees",
                newName: "ElectricFee");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameTable(
                name: "AllCourses",
                newName: "DetailCourse");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_Type",
                table: "Log",
                newName: "IX_Log_Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preference",
                table: "Preference",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyCourse",
                table: "MyCourse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricFee",
                table: "ElectricFee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailCourse",
                table: "DetailCourse",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Preference",
                table: "Preference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyCourse",
                table: "MyCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricFee",
                table: "ElectricFee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCourse",
                table: "DetailCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Preference",
                newName: "Preferences");

            migrationBuilder.RenameTable(
                name: "MyCourse",
                newName: "MyCourses");

            migrationBuilder.RenameTable(
                name: "Log",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "ElectricFee",
                newName: "ElectricFees");

            migrationBuilder.RenameTable(
                name: "DetailCourse",
                newName: "AllCourses");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Log_Type",
                table: "Logs",
                newName: "IX_Logs_Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyCourses",
                table: "MyCourses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricFees",
                table: "ElectricFees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllCourses",
                table: "AllCourses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");
        }
    }
}
