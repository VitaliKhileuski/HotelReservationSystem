using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class Attachments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_FileContentId",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileContentId",
                table: "Attachments",
                column: "FileContentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_FileContentId",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileContentId",
                table: "Attachments",
                column: "FileContentId");
        }
    }
}
