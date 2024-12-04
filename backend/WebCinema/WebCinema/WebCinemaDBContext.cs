using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema
{
    public class WebCinemaDBContext : DbContext
    {
        public WebCinemaDBContext(DbContextOptions<WebCinemaDBContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Genres> Genres { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genres>().HasData(
                new Genres { Id = 1, Name = "Horror" },
                new Genres { Id = 2, Name = "Action" },
                new Genres { Id = 3, Name = "Thriller" },
                new Genres { Id = 4, Name = "Western" },
                new Genres { Id = 5, Name = "Romance" }
                //dodat ostale zanrove pa samo onda: add-migration GenreData2 -> update-database
                );
        }
    }
}