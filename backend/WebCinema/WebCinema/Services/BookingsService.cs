using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly ILogger<BookingsService> _logger;
        private readonly WebCinemaDBContext _dbContext;
        public BookingsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Bookings> CreateBookingsAsync(Bookings bookings)
        {
            if (bookings == null)
            {
                return null;
            }
            await _dbContext.Bookings.AddAsync(bookings);
            await _dbContext.SaveChangesAsync();
            return bookings;
        }

        public async Task<Bookings> DeleteBookingsByIdAsync(int id)
        {
            var bookings = await GetBookingsByIdAsync(id);
            if (bookings != null)
            {
                _dbContext.Bookings.Remove(bookings);
                await _dbContext.SaveChangesAsync();
            }
            return bookings;
        }

        public async Task<List<Bookings>> GetAllBookingsAsync()
        {
            var bookings = await _dbContext.Bookings.ToListAsync();

            return bookings;
        }

        public async Task<Bookings> GetBookingsByIdAsync(int id)
        {
            var bookings = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            return bookings;
        }

        public async Task<Bookings> UpdateBookingsAsync(int id, Bookings bookings)
        {
            var _bookings = await GetBookingsByIdAsync(id);
            if (bookings != null)
            {
                _bookings.BookingDateTime=bookings.BookingDateTime;
                _bookings.BookingStatus=bookings.BookingStatus;
                _bookings.TotalPrice=bookings.TotalPrice;
                _dbContext.Bookings.Update(_bookings);
                await _dbContext.SaveChangesAsync();
            }
            return _bookings;
        }

        public async Task<Bookings> UpdateBookingsBasicInfoAsync(int id, BookingsEditDTO dto)
        {
            try
            {
                var booking = await _dbContext.Bookings.FindAsync(id);
                if (booking == null)
                {
                    _logger.LogWarning($"Booking not found for update: {id}");
                    return null;
                }

                // Optional: Add duplicate checking if needed
                // For example, checking for duplicate bookings might depend on your specific business logic
                var duplicateBooking = await _dbContext.Bookings
                    .FirstOrDefaultAsync(b =>
                        b.BookingDateTime == dto.BookingDateTime &&
                        b.Id != id);
                if (duplicateBooking != null)
                {
                    _logger.LogWarning($"Booking update failed: Booking with same date already exists");
                    throw new InvalidOperationException("Booking with same date already exists");
                }

                // Update booking properties
                booking.ShowTimes = dto.ShowTimes;
                booking.BookingDateTime = dto.BookingDateTime;
                booking.TotalPrice = dto.TotalPrice;
                booking.BookingStatus = dto.BookingStatus;

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Booking updated successfully: {id}");
                return booking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating booking for ID: {id}");
                throw;
            }
        }
    }
}
