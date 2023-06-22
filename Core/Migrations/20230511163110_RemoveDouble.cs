using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class RemoveDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Food",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Foods");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AccountNumber",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Food",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Foods",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
