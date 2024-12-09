using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ISeatsService
    {
        Task<Seats> CreateSeatsAsync(Seats seats);

        Task<List<Seats>> GetAllSeatsAsync();

        Task<Seats> GetSeatsByIdAsync(int id);

        Task<Seats> DeleteSeatsByIdAsync(int id);

        Task<Seats> UpdateSeatsAsync(int id, Seats seats);
    }
}
