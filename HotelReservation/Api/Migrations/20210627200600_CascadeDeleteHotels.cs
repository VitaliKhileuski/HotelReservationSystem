using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class CascadeDeleteHotels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
