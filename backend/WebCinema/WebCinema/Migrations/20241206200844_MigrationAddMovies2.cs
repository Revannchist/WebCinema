using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAddMovies2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Genres_GenresId",
                table: "MoviesGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Movies_MoviesId",
                table: "MoviesGenres");

            migrationBuilder.RenameColumn(
                name: "GenresId",
                table: "MoviesGenres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "MoviesGenres",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesGenres_GenresId",
                table: "MoviesGenres",
                newName: "IX_MoviesGenres_GenreId");

            migrationBuilder.CreateTable(
                name: "MoviesActors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesActors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MoviesActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesActors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_ActorId",
                table: "MoviesActors",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Genres_GenreId",
                table: "MoviesGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Movies_MovieId",
                table: "MoviesGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Genres_GenreId",
                table: "MoviesGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Movies_MovieId",
                table: "MoviesGenres");

            migrationBuilder.DropTable(
                name: "MoviesActors");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "MoviesGenres",
                newName: "GenresId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MoviesGenres",
                newName: "MoviesId");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesGenres_GenreId",
                table: "MoviesGenres",
                newName: "IX_MoviesGenres_GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Genres_GenresId",
                table: "MoviesGenres",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Movies_MoviesId",
                table: "MoviesGenres",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
