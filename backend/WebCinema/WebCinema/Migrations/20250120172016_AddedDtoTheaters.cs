using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class AddedDtoTheaters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booked_Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedSeats_Bookings_BookingId",
                table: "BookedSeats",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedSeats_Seats_SeatsId",
                table: "BookedSeats",
                column: "SeatsId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedSeats_Bookings_BookingId",
                table: "BookedSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_BookedSeats_Seats_SeatsId",
                table: "BookedSeats");

            migrationBuilder.CreateTable(
                name: "Booked_Seats",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    SeatsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Booked_Seats_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Booked_Seats_Seats_SeatsId",
                        column: x => x.SeatsId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }
    }
}
