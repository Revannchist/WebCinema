using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class novaBazaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Theaters_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_Directors_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Directors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatersID = table.Column<int>(type: "int", nullable: false),
                    HallName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    HallType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Halls_Theaters_TheatersID",
                        column: x => x.TheatersID,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviePoster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    PosterImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviePoster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoviePoster_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "MoviesGenres",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MoviesGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoviesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ImageByteArray = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallsId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Halls_HallsId",
                        column: x => x.HallsId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoviesId = table.Column<int>(type: "int", nullable: false),
                    HallsId = table.Column<int>(type: "int", nullable: false),
                    ShowDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowTimes_Halls_HallsId",
                        column: x => x.HallsId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowTimes_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersId = table.Column<int>(type: "int", nullable: false),
                    ShowTimesId = table.Column<int>(type: "int", nullable: false),
                    TicketQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_ShowTimes_ShowTimesId",
                        column: x => x.ShowTimesId,
                        principalTable: "ShowTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookedSeats",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SeatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSeats", x => new { x.BookingId, x.SeatsId });
                    table.ForeignKey(
                        name: "FK_BookedSeats_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookedSeats_Seats_SeatsId",
                        column: x => x.SeatsId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionID = table.Column<int>(type: "int", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Timothee", "Chalamet" },
                    { 2, "Rebecca", "Ferguson" },
                    { 3, "Oscar", "Isaac" },
                    { 4, "Russel", "Crowe" },
                    { 5, "Joaquin", "Phoenix" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mostar" },
                    { 2, "Sarajevo" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "United States" },
                    { 2, "Canada" },
                    { 3, "Germany" },
                    { 4, "United Kingdom" },
                    { 5, "France" }
                });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Ridley", "Scott" },
                    { 2, "Denis", "Villeneuve" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Horror" },
                    { 2, "Action" },
                    { 3, "Thriller" },
                    { 4, "Drama" },
                    { 5, "Science Fiction" },
                    { 6, "Historical" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Moderator" }
                });

            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "Id", "Adress", "CityId", "Name", "PhoneNumber", "PostalCode" },
                values: new object[,]
                {
                    { 1, "Adress1", 1, "Teatar1", "061 467 946", "88000" },
                    { 2, "Adress2", 1, "Teatar2", "061 675 875", "88000" },
                    { 3, "Adress3", 2, "Teatar3", "061 864 079", "71000" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "Password", "RegistrationTime", "RoleId", "RolesId", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "Admin", "User", "SecurePass123", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "adminUser" },
                    { 2, new DateTime(1995, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", "Basic", "User", "UserPass456", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "basicUser" }
                });

            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "Capacity", "HallName", "HallType", "TheatersID" },
                values: new object[,]
                {
                    { 1, 60, "Hall1", "Medium", 1 },
                    { 2, 90, "Hall2", "Big", 1 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_SeatsId",
                table: "BookedSeats",
                column: "SeatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowTimesId",
                table: "Bookings",
                column: "ShowTimesId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UsersId",
                table: "Bookings",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Halls_TheatersID",
                table: "Halls",
                column: "TheatersID");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePoster_MovieId",
                table: "MoviePoster",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryId",
                table: "Movies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_DirectorId",
                table: "Movies",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_ActorId",
                table: "MoviesActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesGenres_GenreId",
                table: "MoviesGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MoviesId_UsersId",
                table: "Ratings",
                columns: new[] { "MoviesId", "UsersId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UsersId",
                table: "Ratings",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_HallsId",
                table: "Seats",
                column: "HallsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_HallsId",
                table: "ShowTimes",
                column: "HallsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_MoviesId",
                table: "ShowTimes",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_Theaters_CityId",
                table: "Theaters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolesId",
                table: "Users",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersImages_UserId",
                table: "UsersImages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedSeats");

            migrationBuilder.DropTable(
                name: "MoviePoster");

            migrationBuilder.DropTable(
                name: "MoviesActors");

            migrationBuilder.DropTable(
                name: "MoviesGenres");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "UsersImages");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ShowTimes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Theaters");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
