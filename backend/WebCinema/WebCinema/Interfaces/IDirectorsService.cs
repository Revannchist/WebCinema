using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IDirectorsService
    {
        Directors CreateDirector(Directors director);

        List<Directors> GetAllDirectors();

        Directors GetDirectorById(int id);

        Directors DeleteDirectorById(int id);

        Directors UpdateDirector(int id, Directors directors);
    }
}
