using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OhMyWhut.Win.Migrations
{
    public partial class ForgetElectricFeeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricFee",
                table: "ElectricFee");

            migrationBuilder.RenameTable(
                name: "ElectricFee",
                newName: "ElectricFees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricFees",
                table: "ElectricFees",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ElectricFees",
                table: "ElectricFees");

            migrationBuilder.RenameTable(
                name: "ElectricFees",
                newName: "ElectricFee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElectricFee",
                table: "ElectricFee",
                column: "Id");
        }
    }
}
