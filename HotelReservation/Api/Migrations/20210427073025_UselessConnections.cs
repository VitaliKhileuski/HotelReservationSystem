using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class UselessConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Locations_LocationId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_LocationId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Hotels");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_HotelId",
                table: "Locations",
                column: "HotelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Hotels_HotelId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_HotelId",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_LocationId",
                table: "Hotels",
                column: "LocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Locations_LocationId",
                table: "Hotels",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
