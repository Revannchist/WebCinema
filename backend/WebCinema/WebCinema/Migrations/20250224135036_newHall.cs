using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class newHall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "Capacity", "HallName", "HallType", "TheatersID" },
                values: new object[,]
                {
                    { 1, 40, "Hall1", "Medium", 1 },
                    { 2, 60, "Hall2", "Big", 1 }
                });

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Adress",
                value: "Adress1");

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 2,
                column: "Adress",
                value: "Adress2");

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 3,
                column: "Adress",
                value: "Adress3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 1,
                column: "Adress",
                value: "Ulica1");

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 2,
                column: "Adress",
                value: "Ulica2");

            migrationBuilder.UpdateData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 3,
                column: "Adress",
                value: "Ulica3");
        }
    }
}
