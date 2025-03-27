using Microsoft.EntityFrameworkCore;
using System.Data;
using WebCinema.Models;

namespace WebCinema
{
    public class WebCinemaDBContext : DbContext
    {
        public WebCinemaDBContext(DbContextOptions<WebCinemaDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder); --nez treba li nam ovo

            modelBuilder.Entity<Genres>().HasData(
                new Genres { Id = 1, Name = "Horror" },
                new Genres { Id = 2, Name = "Action" },
                new Genres { Id = 3, Name = "Thriller" },
                new Genres { Id = 4, Name = "Drama" },
                new Genres { Id = 5, Name = "Science Fiction" },
                new Genres { Id = 6, Name = "Historical" }

                );

            modelBuilder.Entity<Cities>().HasData(
                new Cities { Id = 1, Name = "Mostar" },
                new Cities { Id = 2, Name = "Sarajevo" }
                );

            modelBuilder.Entity<Countries>().HasData(
                new Countries { Id = 1, Name = "United States" },
                new Countries { Id = 2, Name = "Canada" },
                new Countries { Id = 3, Name = "Germany" },
                new Countries { Id = 4, Name = "United Kingdom" },
                new Countries { Id = 5, Name = "France" }
                );

            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, Name = "Admin" },
                new Roles { Id = 2, Name = "User" },
                new Roles { Id = 3, Name = "Moderator" }
                );

            modelBuilder.Entity<Theaters>().HasData(
                new Theaters { Id = 1, Name = "Teatar1", CityId = 1, Adress = "Adress1", PostalCode = "88000", PhoneNumber = "061 467 946" },
                new Theaters { Id = 2, Name = "Teatar2", CityId = 1, Adress = "Adress2", PostalCode = "88000", PhoneNumber = "061 675 875" },
                new Theaters { Id = 3, Name = "Teatar3", CityId = 2, Adress = "Adress3", PostalCode = "71000", PhoneNumber = "061 864 079" }
                );

            modelBuilder.Entity<Halls>().HasData(
                new Halls { Id = 1, TheatersID = 1, HallName = "Hall1", Capacity = 60, HallType = "Medium" },
                new Halls { Id = 2, TheatersID = 1, HallName = "Hall2", Capacity = 90, HallType = "Big" }
                );

            modelBuilder.Entity<Directors>().HasData(
                new Directors { Id = 1, FirstName = "Ridley", LastName = "Scott" },
                new Directors { Id = 2, FirstName = "Denis", LastName = "Villeneuve" }
                );

            modelBuilder.Entity<Actors>().HasData(
                new Actors { Id = 1, FirstName = "Timothee", LastName = "Chalamet" },
                new Actors { Id = 2, FirstName = "Rebecca", LastName = "Ferguson" },
                new Actors { Id = 3, FirstName = "Oscar", LastName = "Isaac" },
                new Actors { Id = 4, FirstName = "Russel", LastName = "Crowe" },
                new Actors { Id = 5, FirstName = "Joaquin", LastName = "Phoenix" }
                );

            modelBuilder.Entity<Users>().HasData(
                new Users //Admin User
                {
                    Id = 1,
                    Username = "adminUser",
                    Email = "admin@example.com",
                    Password = "SecurePass123",
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    RoleId = 1
                },
                new Users //BasicUser
                {
                    Id = 2,
                    Username = "basicUser",
                    Email = "user@example.com",
                    Password = "UserPass456",
                    FirstName = "Basic",
                    LastName = "User",
                    DateOfBirth = new DateTime(1995, 8, 22),
                    RoleId = 2
                }
                );


            // Setup variables
            var seatList = new List<Seats>();
            int seatId = 1;

            // Hall 1: 4 rows of 15 seats (total 60 seats)
            for (int row = 1; row <= 4; row++)
            {
                for (int seatNum = 1; seatNum <= 15; seatNum++)
                {
                    string seatType = "Regular";

                    // Define Love seats
                    if ((row == 1 || row == 2) && (seatNum == 7 || seatNum == 8))
                    {
                        seatType = "Love";
                    }
                    else if (row == 3 && (seatNum >= 6 && seatNum <= 9))
                    {
                        seatType = "Love";
                    }

                    // Define Accessible seats
                    if (row == 4 && (seatNum == 1 || seatNum == 2 || seatNum >= 13))
                    {
                        seatType = "Accessible";
                    }

                    // Calculate absolute seat number (1-15 for row 1, 16-30 for row 2, etc.)
                    int absoluteSeatNum = ((row - 1) * 15) + seatNum;

                    seatList.Add(new Seats
                    {
                        Id = seatId++,
                        HallsId = 1,
                        SeatNumber = absoluteSeatNum,  // Using absolute seat number
                        SeatType = seatType
                    });
                }
            }

            // Hall 2: 6 rows of 15 seats (total 90 seats)
            for (int row = 1; row <= 6; row++)
            {
                for (int seatNum = 1; seatNum <= 15; seatNum++)
                {
                    string seatType = "Regular";

                    // Define Love seats
                    if (row == 2 && ((seatNum == 4 || seatNum == 5) || (seatNum == 11 || seatNum == 12)))
                    {
                        seatType = "Love";
                    }
                    else if (row == 4 && (seatNum >= 5 && seatNum <= 8))
                    {
                        seatType = "Love";
                    }

                    // Define Accessible seats
                    if (row == 6 && (seatNum >= 1 && seatNum <= 3))
                    {
                        seatType = "Accessible";
                    }
                    else if (row == 5 && (seatNum >= 13 && seatNum <= 15))
                    {
                        seatType = "Accessible";
                    }

                    // Calculate absolute seat number
                    int absoluteSeatNum = ((row - 1) * 15) + seatNum;

                    seatList.Add(new Seats
                    {
                        Id = seatId++,
                        HallsId = 2,
                        SeatNumber = absoluteSeatNum,  // Using absolute seat number
                        SeatType = seatType
                    });
                }
            }

            modelBuilder.Entity<Seats>().HasData(seatList.ToArray());


            //------------------------------------------------------//
            modelBuilder.Entity<MoviesGenres>()
                .HasKey(sc => new { sc.MovieId, sc.GenreId });

            modelBuilder.Entity<MoviesGenres>()
                .HasOne(sc => sc.Movie)
                .WithMany(s => s.MoviesGenres)
                .HasForeignKey(sc => sc.MovieId);

            modelBuilder.Entity<MoviesGenres>()
                .HasOne(sc => sc.Genre)
                .WithMany(c => c.MoviesGenres)
                .HasForeignKey(sc => sc.GenreId);
            //------------------------------------------------------//
            modelBuilder.Entity<MoviesActors>()
                .HasKey(sc => new { sc.MovieId, sc.ActorId });

            modelBuilder.Entity<MoviesActors>()
                .HasOne(sc => sc.Movie)
                .WithMany(s => s.MoviesActors)
                .HasForeignKey(sc => sc.MovieId);

            modelBuilder.Entity<MoviesActors>()
                .HasOne(sc => sc.Actor)
                .WithMany(c => c.MoviesActors)
                .HasForeignKey(sc => sc.ActorId);
            //------------------------------------------------------//

            modelBuilder.Entity<Movies>()
                .HasOne(b => b.Director)
                .WithMany(a => a.Movie)
                .HasForeignKey(b => b.DirectorId)
                .OnDelete(DeleteBehavior.SetNull);
            //------------------------------------------------------//
            modelBuilder.Entity<Directors>()
                .HasMany(d => d.Movie)
                .WithOne(m => m.Director)
                .HasForeignKey(m => m.DirectorId);
            //------------------------------------------------------//

            modelBuilder.Entity<BookedSeats>()
                .HasKey(bs => new { bs.BookingId, bs.SeatsId });

            modelBuilder.Entity<BookedSeats>()
                .HasOne(bs => bs.Bookings)
                .WithMany(b => b.BookedSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict); // Ovdje Restrict umjesto CASCADE

            modelBuilder.Entity<BookedSeats>()
                .HasOne(bs => bs.Seats)
                .WithMany(s => s.BookedSeats)
                .HasForeignKey(bs => bs.SeatsId)
                .OnDelete(DeleteBehavior.Restrict);
            //------------------------------------------------------//
            modelBuilder.Entity<Ratings>()
                .HasIndex(r => new { r.MoviesId, r.UsersId })
                .IsUnique();
            //------------------------------------------------------//
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Roles)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            //------------------------------------------------------//



        }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Directors> Directors { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Theaters>Theaters { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<Halls>Halls { get; set; }
        public DbSet<Seats>Seats { get; set; }
        public DbSet<ShowTimes>ShowTimes { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Payments>Payments { get; set; }
        public DbSet<MoviePoster> MoviePoster { get; set; }
        public DbSet<UsersImage>UsersImages { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<BookedSeats> BookedSeats { get; set; } 

    }
}