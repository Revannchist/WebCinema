using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class GenresService : IGenresService
    {
        private readonly WebCinemaDBContext _dbContext;

        public GenresService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genres> GetGenresByIdAsync(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            return genre;
        }

        public async Task<List<Genres>> GetAllGenresAsync() 
        {
            var genres = await _dbContext.Genres.ToListAsync();
            return genres;
        }
    }
}
