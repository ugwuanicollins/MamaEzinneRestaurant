using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addSalesRecordToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evidence",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "SalesRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FoodId = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    GrandTotal = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesRecords_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesRecords_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_FoodId",
                table: "SalesRecords",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_UserId",
                table: "SalesRecords",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesRecords");

            migrationBuilder.AddColumn<string>(
                name: "Evidence",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
