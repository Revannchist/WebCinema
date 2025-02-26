using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IBookingsService
    {
        Task<Bookings> CreateBookingsAsync(Bookings bookings);

        Task<List<BookingsDto>> GetAllBookingsAsync();

        //Task<BookingsDto> GetBookingsByIdAsync(int id);

        Task<bool> DeleteBookingsByIdAsync(int id);

        //Task<Bookings> UpdateBookingsAsync(int id, Bookings bookings);

        //Task<Bookings>UpdateBookingsBasicInfoAsync(int id,BookingsEditDto dto);
    }
}
