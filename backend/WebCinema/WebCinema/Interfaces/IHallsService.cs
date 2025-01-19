using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IHallsService
    {
        Task<Halls> CreateHallsAsync(Halls halls);

        Task<List<HallDisplayDto>> GetAllHallsAsync();

        Task<HallDisplayDto> GetHallsByIdAsync(int id);

        Task<HallDisplayDto> DeleteHallsByIdAsync(int id);

        Task<HallDisplayDto> UpdateHallsAsync(int id, Halls halls);
        Task<Halls> GetHallEntityByIdAsync(int id);
    }
}
