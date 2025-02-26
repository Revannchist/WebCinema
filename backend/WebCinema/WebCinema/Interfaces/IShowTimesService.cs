using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IShowTimesService
    {
        Task<ShowTimes> CreateShowTimesAsync(ShowTimes showtimes);

        Task<List<ShowTimesDto>> GetAllShowTimesAsync();

        Task<ShowTimesDto> GetShowTimesByIdAsync(int id);

        Task<ShowTimes> DeleteShowTimesByIdAsync(int id);

        Task<ShowTimesDto?> UpdateShowTimesAsync(int id, ShowTimesUpdateDto updateDto);
    }
}
