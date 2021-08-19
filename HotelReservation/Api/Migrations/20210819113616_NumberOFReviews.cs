using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class NumberOFReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfReviews",
                table: "AverageHotelRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfReviews",
                table: "AverageHotelRatings");
        }
    }
}
