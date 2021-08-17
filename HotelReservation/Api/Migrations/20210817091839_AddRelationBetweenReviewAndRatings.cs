using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AddRelationBetweenReviewAndRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Reviews_ReviewEntityId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ReviewEntityId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ReviewEntityId",
                table: "Ratings");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ReviewId",
                table: "Ratings",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Reviews_ReviewId",
                table: "Ratings",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Reviews_ReviewId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ReviewId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Ratings");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewEntityId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ReviewEntityId",
                table: "Ratings",
                column: "ReviewEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Reviews_ReviewEntityId",
                table: "Ratings",
                column: "ReviewEntityId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
