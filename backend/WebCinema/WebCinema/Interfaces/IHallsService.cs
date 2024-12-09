using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IHallsService
    {
        Task<Halls> CreateHallsAsync(Halls halls);

        Task<List<Halls>> GetAllHallsAsync();

        Task<Halls> GetHallsByIdAsync(int id);

        Task<Halls> DeleteHallsByIdAsync(int id);

        Task<Halls> UpdateHallsAsync(int id, Halls halls);
    }
}
