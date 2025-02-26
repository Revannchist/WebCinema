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

            var showTimes = await _dbContext.ShowTimes.FindAsync(bookings.ShowTimesId);
            if (showTimes == null)
            {
                throw new InvalidOperationException($"ShowTime with ID {bookings.ShowTimesId} does not exist.");
            }

            if (bookings.BookedSeats != null && bookings.BookedSeats.Count > 0)
            {
                bookings.TicketQuantity = bookings.BookedSeats.Count;
                decimal ticketPrice = showTimes.TicketPrice;
                bookings.TotalPrice = bookings.TicketQuantity * ticketPrice;

                foreach (var bookedSeat in bookings.BookedSeats)
                {
                    var seat = await _dbContext.Seats.FindAsync(bookedSeat.SeatsId);
                    if (seat == null)
                    {
                        throw new InvalidOperationException($"Seat {bookedSeat.SeatsId} does not exist.");
                    }

                    var existingBooking = await _dbContext.BookedSeats
                        .FirstOrDefaultAsync(bs => bs.SeatsId == bookedSeat.SeatsId &&
                                                   bs.Bookings.ShowTimesId == bookings.ShowTimesId);
                    if (existingBooking != null)
                    {
                        throw new InvalidOperationException($"Seat {bookedSeat.SeatsId} is already booked for this showtime.");
                    }
                }
            }

            await _dbContext.Bookings.AddAsync(bookings);
            await _dbContext.SaveChangesAsync();
            return bookings;
        }

        public async Task<bool> DeleteBookingsByIdAsync(int id)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.BookedSeats)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (booking != null)
            {
                if (booking.BookedSeats != null)
                {
                    _dbContext.BookedSeats.RemoveRange(booking.BookedSeats);
                }
                _dbContext.Bookings.Remove(booking);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<BookingsDto>> GetAllBookingsAsync()
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.ShowTimes)
                    .ThenInclude(s => s.Movies)
                .Include(b => b.ShowTimes)
                    .ThenInclude(s => s.Halls)
                .Include(b => b.BookedSeats)
                    .ThenInclude(bs => bs.Seats)
                .Select(b => new BookingsDto
                {
                    Id = b.Id,
                    UserName = b.User.Username,
                    MovieTitle = b.ShowTimes.Movies.Title,
                    HallName = b.ShowTimes.Halls.HallName,
                    ShowDateTime = b.ShowTimes.ShowDateTime,
                    BookingDate = b.BookingDate,
                    TicketQuantity = b.TicketQuantity,
                    TotalPrice = b.TotalPrice,
                    BookingStatus = b.BookingStatus,
                    BookedSeats = b.BookedSeats.Select(bs => bs.Seats.SeatNumber).ToList() // Seat numbers only
                })
                .ToListAsync();

            return bookings;
        }




        //public async Task<BookingsDto> GetBookingsByIdAsync(int id)
        //{
        //    var booking = await _dbContext.Bookings
        //        .Include(b => b.User)
        //        .Include(b => b.ShowTimes)
        //            .ThenInclude(s => s.Movies)
        //        .Include(b => b.ShowTimes)
        //            .ThenInclude(s => s.Halls)
        //        .Where(b => b.Id == id)
        //        .Select(b => new BookingsDto
        //        {
        //            Id = b.Id,
        //            UserName = b.User.Username,
        //            MovieTitle = b.ShowTimes.Movies.Title,
        //            HallName = b.ShowTimes.Halls.HallName,
        //            ShowDateTime = b.ShowTimes.ShowDateTime,
        //            BookingDateTime = b.BookingDateTime,
        //            TotalPrice = b.TotalPrice,
        //            BookingStatus = b.BookingStatus
        //        })
        //        .FirstOrDefaultAsync();

        //    return booking;
        //}

        //public async Task<Bookings> UpdateBookingsAsync(int id, Bookings bookings)
        //{
        //    var existingBooking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == id);
        //    if (existingBooking != null)
        //    {
        //        existingBooking.BookingDateTime = bookings.BookingDateTime;
        //        existingBooking.BookingStatus = bookings.BookingStatus;
        //        existingBooking.TotalPrice = bookings.TotalPrice;
        //        _dbContext.Bookings.Update(existingBooking);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    return existingBooking;
        //}

        //public async Task<Bookings> UpdateBookingsBasicInfoAsync(int id, BookingsEditDto dto)
        //{
        //    try
        //    {
        //        var booking = await _dbContext.Bookings.FindAsync(id);
        //        if (booking == null)
        //        {
        //            _logger.LogWarning($"Booking not found for update: {id}");
        //            return null;
        //        }

        //        // Optional: Add duplicate checking if needed
        //        // For example, checking for duplicate bookings might depend on your specific business logic
        //        var duplicateBooking = await _dbContext.Bookings
        //            .FirstOrDefaultAsync(b =>
        //                b.BookingDateTime == dto.BookingDateTime &&
        //                b.Id != id);
        //        if (duplicateBooking != null)
        //        {
        //            _logger.LogWarning($"Booking update failed: Booking with same date already exists");
        //            throw new InvalidOperationException("Booking with same date already exists");
        //        }

        //        // Update booking properties
        //        booking.ShowTimes = dto.ShowTimes;
        //        booking.BookingDateTime = dto.BookingDateTime;
        //        booking.TotalPrice = dto.TotalPrice;
        //        booking.BookingStatus = dto.BookingStatus;

        //        await _dbContext.SaveChangesAsync();
        //        _logger.LogInformation($"Booking updated successfully: {id}");
        //        return booking;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error updating booking for ID: {id}");
        //        throw;
        //    }
        //}
    }
}
