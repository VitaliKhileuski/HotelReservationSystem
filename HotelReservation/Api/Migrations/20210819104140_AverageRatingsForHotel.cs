using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AverageRatingsForHotel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AverageHotelRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageHotelRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageHotelRatings_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AverageHotelRatings_ReviewCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ReviewCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AverageHotelRatings_CategoryId",
                table: "AverageHotelRatings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AverageHotelRatings_HotelId",
                table: "AverageHotelRatings",
                column: "HotelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AverageHotelRatings");
        }
    }
}
