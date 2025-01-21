using WebCinema.Models.DTO;
using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ITheatersService
    {
        Task<Theaters> CreateTheatersAsync(Theaters theaters);

        Task<List<TheaterDto>> GetAllTheatersAsync();

        Task<TheaterDto> GetTheatersByIdAsync(int id);

        Task<Theaters> DeleteTheatersByIdAsync(int id);

        Task<Theaters> UpdateTheatersAsync(int id, Theaters theaters);
    }
}
