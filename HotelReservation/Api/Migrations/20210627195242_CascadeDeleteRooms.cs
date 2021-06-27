using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class CascadeDeleteRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Hotels_HotelEntityId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Rooms_RoomEntityId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_HotelEntityId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_RoomEntityId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "HotelEntityId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "RoomEntityId",
                table: "Attachments");

            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_HotelId",
                table: "Attachments",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_RoomId",
                table: "Attachments",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Rooms_RoomId",
                table: "Attachments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Hotels_HotelId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Rooms_RoomId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_HotelId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_RoomId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Attachments");

            migrationBuilder.AddColumn<Guid>(
                name: "HotelEntityId",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomEntityId",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_HotelEntityId",
                table: "Attachments",
                column: "HotelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_RoomEntityId",
                table: "Attachments",
                column: "RoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Hotels_HotelEntityId",
                table: "Attachments",
                column: "HotelEntityId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Rooms_RoomEntityId",
                table: "Attachments",
                column: "RoomEntityId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
