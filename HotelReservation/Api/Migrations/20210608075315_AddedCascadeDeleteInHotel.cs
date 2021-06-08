using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AddedCascadeDeleteInHotel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Hotels_HotelId",
                table: "Services",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
