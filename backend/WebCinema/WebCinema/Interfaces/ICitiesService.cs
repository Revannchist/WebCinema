//using WebCinema.Migrations;
using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ICitiesService
    {
        Task<Cities> GetCitiesByIdAsync(int id);

        Task<List<Cities>> GetCitiesAsync();
    }
}
