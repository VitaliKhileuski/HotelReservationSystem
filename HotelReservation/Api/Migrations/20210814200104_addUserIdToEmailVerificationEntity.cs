using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class addUserIdToEmailVerificationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmailVerificationEntities");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EmailVerificationEntities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailVerificationEntities");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmailVerificationEntities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
