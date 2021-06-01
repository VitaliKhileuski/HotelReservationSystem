using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AddHotelUserConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelAdminId",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Hotels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AdminId",
                table: "Hotels",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Users_AdminId",
                table: "Hotels",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Users_AdminId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_AdminId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "HotelAdminId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }
    }
}
