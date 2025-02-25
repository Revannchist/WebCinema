using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class newTheater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "Id", "Adress", "CityId", "Name", "PhoneNumber", "PostalCode" },
                values: new object[,]
                {
                    { 1, "Ulica1", 1, "Teatar1", "061 467 946", "88000" },
                    { 2, "Ulica2", 1, "Teatar2", "061 675 875", "88000" },
                    { 3, "Ulica3", 2, "Teatar3", "061 864 079", "71000" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
