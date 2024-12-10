using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class AddBookedSeatsMigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booked_Seats",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SeatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booked_Seats", x => new { x.BookingId, x.SeatsId });
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

            migrationBuilder.CreateIndex(
                name: "IX_Booked_Seats_SeatsId",
                table: "Booked_Seats",
                column: "SeatsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booked_Seats");
        }
    }
}
