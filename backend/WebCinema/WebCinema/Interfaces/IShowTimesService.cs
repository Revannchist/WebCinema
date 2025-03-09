using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IShowTimesService
    {
        Task<ShowTimes> CreateShowTimesAsync(ShowTimes showtimes, CancellationToken cancellationToken = default);

        Task<List<ShowTimesDto>> GetAllShowTimesAsync(CancellationToken cancellationToken = default);

        Task<ShowTimesDto> GetShowTimesByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ShowTimes> DeleteShowTimesByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ShowTimesDto?> UpdateShowTimesAsync(int id, ShowTimesUpdateDto updateDto, CancellationToken cancellationToken = default);
    }
}
