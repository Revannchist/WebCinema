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
                new Genres { Id = 4, Name = "Western" },
                new Genres { Id = 5, Name = "Romance" }
                //dodat ostale zanrove pa samo onda: add-migration GenreData2 -> update-database
                );

            modelBuilder.Entity<Cities>().HasData(
                new Cities { Id = 1, Name = "Mostar" },
                new Cities { Id = 2, Name = "Sarajevo" }
                );

            modelBuilder.Entity<Countries>().HasData(
                new Countries { Id = 1, Name = "BiH" },
                new Countries { Id = 2, Name = "Germany" }
                );

            modelBuilder.Entity<Actors>().HasData(
                new Actors { Id = 1, FirstName = "Brad", LastName = "Pitt" },
                new Actors { Id = 2, FirstName = "Angelina", LastName = "Jolie" }
                );

            modelBuilder.Entity<Directors>().HasData(
                new Directors { Id = 1, FirstName = "Hilk", LastName = "Hogan" },
                new Directors { Id = 2, FirstName = "Christopher", LastName = "Nolan" }
                );
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
            //------------------------------------------------------//-------------------//

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

            modelBuilder.Entity<Booked_Seats>()
                .HasKey(bs => new { bs.BookingId, bs.SeatsId });

            modelBuilder.Entity<Booked_Seats>()
                .HasOne(bs => bs.Bookings)
                .WithMany(b => b.Booked_Seats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict); // Ovdje Restrict umjesto CASCADE

            modelBuilder.Entity<Booked_Seats>()
                .HasOne(bs => bs.Seats)
                .WithMany(s => s.Booked_Seats)
                .HasForeignKey(bs => bs.SeatsId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------------------------//


            modelBuilder.Entity<Roles>().HasData(
            new Roles { Id = 1, Name = "Admin" },
            new Roles { Id = 2, Name = "User" },
            new Roles { Id = 3, Name = "Moderator" }
            );

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
        public DbSet<MoviesImage> MoviesImages { get; set; }
        public DbSet<UsersImage>UsersImages { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}