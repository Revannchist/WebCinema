using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface ISeatsService
    {
        Task<Seats> CreateSeatsAsync(Seats seats);

        Task<List<SeatsDto>> GetAllSeatsAsync();

        Task<SeatsDto> GetSeatsByIdAsync(int id);

        Task<Seats> DeleteSeatsByIdAsync(int id);

        Task<Seats> UpdateSeatsAsync(int id, Seats seats);
    }
}
