using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IBookingsService
    {
        Task<BookingsResponseDto> CreateBookingsAsync(BookingsAddDto bookingsDto, CancellationToken cancellationToken = default);

        Task<List<BookingsDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default);

        Task<BookingsDto> GetBookingsByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> DeleteBookingsByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<BookingsResponseDto> UpdateBookingsAsync(int id, BookingsEditDto bookingsDto, CancellationToken cancellationToken = default);
    }
}
