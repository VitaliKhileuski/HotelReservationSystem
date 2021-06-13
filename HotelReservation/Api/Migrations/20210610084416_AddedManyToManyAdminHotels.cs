using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class AddedManyToManyAdminHotels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Users_AdminId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_AdminId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Hotels");

            migrationBuilder.CreateTable(
                name: "HotelEntityUserEntity",
                columns: table => new
                {
                    AdminsId = table.Column<int>(type: "int", nullable: false),
                    OwnedHotelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelEntityUserEntity", x => new { x.AdminsId, x.OwnedHotelsId });
                    table.ForeignKey(
                        name: "FK_HotelEntityUserEntity_Hotels_OwnedHotelsId",
                        column: x => x.OwnedHotelsId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelEntityUserEntity_Users_AdminsId",
                        column: x => x.AdminsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelEntityUserEntity_OwnedHotelsId",
                table: "HotelEntityUserEntity",
                column: "OwnedHotelsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelEntityUserEntity");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Hotels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_AdminId",
                table: "Hotels",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Users_AdminId",
                table: "Hotels",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
