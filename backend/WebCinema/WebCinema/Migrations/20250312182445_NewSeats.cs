using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class NewSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Drama");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Action");

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Historical" });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "HallsId", "SeatNumber", "SeatType" },
                values: new object[,]
                {
                    { 1, 1, 1, "Regular" },
                    { 2, 1, 2, "Regular" },
                    { 3, 1, 3, "Regular" },
                    { 4, 1, 4, "Regular" },
                    { 5, 1, 5, "Regular" },
                    { 6, 1, 6, "Regular" },
                    { 7, 1, 7, "Love" },
                    { 8, 1, 8, "Love" },
                    { 9, 1, 9, "Regular" },
                    { 10, 1, 10, "Regular" },
                    { 11, 1, 11, "Regular" },
                    { 12, 1, 12, "Regular" },
                    { 13, 1, 13, "Regular" },
                    { 14, 1, 14, "Regular" },
                    { 15, 1, 15, "Regular" },
                    { 16, 1, 16, "Regular" },
                    { 17, 1, 17, "Regular" },
                    { 18, 1, 18, "Regular" },
                    { 19, 1, 19, "Regular" },
                    { 20, 1, 20, "Regular" },
                    { 21, 1, 21, "Regular" },
                    { 22, 1, 22, "Love" },
                    { 23, 1, 23, "Love" },
                    { 24, 1, 24, "Regular" },
                    { 25, 1, 25, "Regular" },
                    { 26, 1, 26, "Regular" },
                    { 27, 1, 27, "Regular" },
                    { 28, 1, 28, "Regular" },
                    { 29, 1, 29, "Regular" },
                    { 30, 1, 30, "Regular" },
                    { 31, 1, 31, "Regular" },
                    { 32, 1, 32, "Regular" },
                    { 33, 1, 33, "Regular" },
                    { 34, 1, 34, "Regular" },
                    { 35, 1, 35, "Regular" },
                    { 36, 1, 36, "Love" },
                    { 37, 1, 37, "Love" },
                    { 38, 1, 38, "Love" },
                    { 39, 1, 39, "Love" },
                    { 40, 1, 40, "Regular" },
                    { 41, 1, 41, "Regular" },
                    { 42, 1, 42, "Regular" },
                    { 43, 1, 43, "Regular" },
                    { 44, 1, 44, "Regular" },
                    { 45, 1, 45, "Regular" },
                    { 46, 1, 46, "Accessible" },
                    { 47, 1, 47, "Accessible" },
                    { 48, 1, 48, "Regular" },
                    { 49, 1, 49, "Regular" },
                    { 50, 1, 50, "Regular" },
                    { 51, 1, 51, "Regular" },
                    { 52, 1, 52, "Regular" },
                    { 53, 1, 53, "Regular" },
                    { 54, 1, 54, "Regular" },
                    { 55, 1, 55, "Regular" },
                    { 56, 1, 56, "Regular" },
                    { 57, 1, 57, "Regular" },
                    { 58, 1, 58, "Accessible" },
                    { 59, 1, 59, "Accessible" },
                    { 60, 1, 60, "Accessible" },
                    { 61, 2, 1, "Regular" },
                    { 62, 2, 2, "Regular" },
                    { 63, 2, 3, "Regular" },
                    { 64, 2, 4, "Regular" },
                    { 65, 2, 5, "Regular" },
                    { 66, 2, 6, "Regular" },
                    { 67, 2, 7, "Regular" },
                    { 68, 2, 8, "Regular" },
                    { 69, 2, 9, "Regular" },
                    { 70, 2, 10, "Regular" },
                    { 71, 2, 11, "Regular" },
                    { 72, 2, 12, "Regular" },
                    { 73, 2, 13, "Regular" },
                    { 74, 2, 14, "Regular" },
                    { 75, 2, 15, "Regular" },
                    { 76, 2, 16, "Regular" },
                    { 77, 2, 17, "Regular" },
                    { 78, 2, 18, "Regular" },
                    { 79, 2, 19, "Love" },
                    { 80, 2, 20, "Love" },
                    { 81, 2, 21, "Regular" },
                    { 82, 2, 22, "Regular" },
                    { 83, 2, 23, "Regular" },
                    { 84, 2, 24, "Regular" },
                    { 85, 2, 25, "Regular" },
                    { 86, 2, 26, "Love" },
                    { 87, 2, 27, "Love" },
                    { 88, 2, 28, "Regular" },
                    { 89, 2, 29, "Regular" },
                    { 90, 2, 30, "Regular" },
                    { 91, 2, 31, "Regular" },
                    { 92, 2, 32, "Regular" },
                    { 93, 2, 33, "Regular" },
                    { 94, 2, 34, "Regular" },
                    { 95, 2, 35, "Regular" },
                    { 96, 2, 36, "Regular" },
                    { 97, 2, 37, "Regular" },
                    { 98, 2, 38, "Regular" },
                    { 99, 2, 39, "Regular" },
                    { 100, 2, 40, "Regular" },
                    { 101, 2, 41, "Regular" },
                    { 102, 2, 42, "Regular" },
                    { 103, 2, 43, "Regular" },
                    { 104, 2, 44, "Regular" },
                    { 105, 2, 45, "Regular" },
                    { 106, 2, 46, "Regular" },
                    { 107, 2, 47, "Regular" },
                    { 108, 2, 48, "Regular" },
                    { 109, 2, 49, "Regular" },
                    { 110, 2, 50, "Love" },
                    { 111, 2, 51, "Love" },
                    { 112, 2, 52, "Love" },
                    { 113, 2, 53, "Love" },
                    { 114, 2, 54, "Regular" },
                    { 115, 2, 55, "Regular" },
                    { 116, 2, 56, "Regular" },
                    { 117, 2, 57, "Regular" },
                    { 118, 2, 58, "Regular" },
                    { 119, 2, 59, "Regular" },
                    { 120, 2, 60, "Regular" },
                    { 121, 2, 61, "Regular" },
                    { 122, 2, 62, "Regular" },
                    { 123, 2, 63, "Regular" },
                    { 124, 2, 64, "Regular" },
                    { 125, 2, 65, "Regular" },
                    { 126, 2, 66, "Regular" },
                    { 127, 2, 67, "Regular" },
                    { 128, 2, 68, "Regular" },
                    { 129, 2, 69, "Regular" },
                    { 130, 2, 70, "Regular" },
                    { 131, 2, 71, "Regular" },
                    { 132, 2, 72, "Regular" },
                    { 133, 2, 73, "Accessible" },
                    { 134, 2, 74, "Accessible" },
                    { 135, 2, 75, "Accessible" },
                    { 136, 2, 76, "Accessible" },
                    { 137, 2, 77, "Accessible" },
                    { 138, 2, 78, "Accessible" },
                    { 139, 2, 79, "Regular" },
                    { 140, 2, 80, "Regular" },
                    { 141, 2, 81, "Regular" },
                    { 142, 2, 82, "Regular" },
                    { 143, 2, 83, "Regular" },
                    { 144, 2, 84, "Regular" },
                    { 145, 2, 85, "Regular" },
                    { 146, 2, 86, "Regular" },
                    { 147, 2, 87, "Regular" },
                    { 148, 2, 88, "Regular" },
                    { 149, 2, 89, "Regular" },
                    { 150, 2, 90, "Regular" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Western");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Romance");
        }
    }
}
