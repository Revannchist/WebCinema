using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IShowTimesService
    {
        Task<ShowTimes> CreateShowTimesAsync(ShowTimes showtimes);

        Task<List<ShowTimes>> GetAllShowTimesAsync();

        Task<ShowTimes> GetShowTimesByIdAsync(int id);

        Task<ShowTimes> DeleteShowTimesByIdAsync(int id);

        Task<ShowTimes> UpdateShowTimesAsync(int id, ShowTimes showtimes);
    }
}
