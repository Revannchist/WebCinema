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

        public Genres GetGenresById(int id)
        {
            var genre = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
            return genre;
        }

        public List<Genres> GetAllGenres() 
        {
            var genres = _dbContext.Genres.ToList();
            return genres;
        }
    }
}
