using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class CheckInAndCheckOutTimesForOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckInTime",
                table: "Orders",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckOutTime",
                table: "Orders",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "Orders");
        }
    }
}
