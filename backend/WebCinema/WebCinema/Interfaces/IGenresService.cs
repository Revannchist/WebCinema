using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IGenresService
    {
        Task<Genres> GetGenresByIdAsync(int id);

        Task<List<Genres>> GetAllGenresAsync() ;
    }
}
