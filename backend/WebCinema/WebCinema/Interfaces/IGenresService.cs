using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IGenresService
    {
        Genres GetGenresById(int id);

        List<Genres> GetAllGenres() ;
    }
}
