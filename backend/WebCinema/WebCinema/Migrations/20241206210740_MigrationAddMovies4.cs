using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAddMovies4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryId",
                table: "Movies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_DirectorId",
                table: "Movies",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Countries_CountryId",
                table: "Movies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Directors_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Countries_CountryId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Directors_DirectorId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CountryId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_DirectorId",
                table: "Movies");
        }
    }
}
