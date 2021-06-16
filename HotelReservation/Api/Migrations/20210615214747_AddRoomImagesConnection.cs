using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AddRoomImagesConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomEntityId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_RoomEntityId",
                table: "Images",
                column: "RoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Rooms_RoomEntityId",
                table: "Images",
                column: "RoomEntityId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Rooms_RoomEntityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_RoomEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "RoomEntityId",
                table: "Images");
        }
    }
}
