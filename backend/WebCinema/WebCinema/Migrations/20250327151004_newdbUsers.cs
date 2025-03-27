using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class newdbUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "Password", "RegistrationTime", "RoleId", "RolesId", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "Admin", "User", "SecurePass123", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "adminUser" },
                    { 2, new DateTime(1995, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", "Basic", "User", "UserPass456", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "basicUser" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
