using Microsoft.EntityFrameworkCore;
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
                .HasForeignKey(b => b.DirectorId);
            //------------------------------------------------------//
            modelBuilder.Entity<Movies>()
                .HasOne(b => b.Country)
                .WithMany(a => a.Movie)
                .HasForeignKey(b => b.CountryId);
            //------------------------------------------------------//




        }
        //pisem DbSet ovdje dole zato jer je tako bilo u Pr3

        public DbSet<Countries> Countries { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Directors> Directors { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Theaters>Theaters { get; set; }
    }
}