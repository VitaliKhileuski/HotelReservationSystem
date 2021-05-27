using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelReservation.Api.Migrations
{
    public partial class DeleteBirthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "admin@gmail.com", "/9M/V3ZgeTgJxwcO01Cc3K8dtoOZbxn7ELlsozJopcs=T_CpC.rqbvX65ycC!dhK4I-0G(QAzSId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Birthdate", "Email", "Password" },
                values: new object[] { new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@gmail.com", "wcIksDzZvHtqhtd/XazkAZF2bEhc1V3EjK+ayHMzXW8=T_CpC.rqbvX65ycC!dhK4I-0G(QAzSId" });
        }
    }
}
