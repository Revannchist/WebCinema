using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema
{
    public class WebCinemaDBContext : DbContext
    {
        public WebCinemaDBContext(DbContextOptions<WebCinemaDBContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder); 
            
        }
    }
}