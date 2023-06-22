using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefNumber",
                table: "Orders",
                newName: "ReferenceNumber");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Payments");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "Orders",
                newName: "RefNumber");
        }
    }
}
