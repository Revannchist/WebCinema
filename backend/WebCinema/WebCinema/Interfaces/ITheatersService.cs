using WebCinema.Models.DTO;
using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ITheatersService
    {
        Task<Theaters> CreateTheatersAsync(Theaters theaters);

        Task<List<Theaters>> GetAllTheatersAsync();

        Task<Theaters> GetTheatersByIdAsync(int id);

        Task<Theaters> DeleteTheatersByIdAsync(int id);

        Task<Theaters> UpdateTheatersAsync(int id, Theaters theaters);
    }
}
