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
        public BookingsService(ILogger<BookingsService> logger, WebCinemaDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<BookingsResponseDto> CreateBookingsAsync(BookingsAddDto bookingsDto, CancellationToken cancellationToken = default)
        {
            if (bookingsDto == null)
            {
                throw new ArgumentNullException(nameof(bookingsDto));
            }

            // Check if showtime exists
            var showTimes = await _dbContext.ShowTimes.FindAsync(new object[] { bookingsDto.ShowTimesId }, cancellationToken);
            if (showTimes == null)
            {
                throw new InvalidOperationException($"ShowTime with ID {bookingsDto.ShowTimesId} does not exist.");
            }

            // Create new booking
            var newBooking = new Bookings
            {
                UsersId = bookingsDto.UsersId,
                ShowTimesId = bookingsDto.ShowTimesId,
                BookingDate = bookingsDto.BookingDate ?? DateTime.Now, // Use current time as default if null
                BookingStatus = bookingsDto.BookingStatus ?? "Pending" // Default status
            };

            // Handle booked seats
            if (bookingsDto.BookedSeatsIds != null && bookingsDto.BookedSeatsIds.Count > 0)
            {
                newBooking.BookedSeats = new List<BookedSeats>();

                foreach (var seatId in bookingsDto.BookedSeatsIds)
                {
                    var seat = await _dbContext.Seats.FindAsync(new object[] { seatId }, cancellationToken);
                    if (seat == null)
                    {
                        throw new InvalidOperationException($"Seat {seatId} does not exist.");
                    }

                    // Check if seat is already booked
                    var existingBookedSeat = await _dbContext.BookedSeats
                        .FirstOrDefaultAsync(bs => bs.SeatsId == seatId &&
                                                  bs.Bookings.ShowTimesId == bookingsDto.ShowTimesId,
                                            cancellationToken);

                    if (existingBookedSeat != null)
                    {
                        throw new InvalidOperationException($"Seat {seatId} is already booked for this showtime.");
                    }

                    // Create a new BookedSeats item
                    var newBookedSeat = new BookedSeats
                    {
                        SeatsId = seatId,
                        Bookings = newBooking  // Using navigation property
                    };

                    newBooking.BookedSeats.Add(newBookedSeat);
                }

                // Calculate ticket quantity and total price
                newBooking.TicketQuantity = newBooking.BookedSeats.Count;
                newBooking.TotalPrice = newBooking.TicketQuantity * showTimes.TicketPrice;
            }
            else if (bookingsDto.TotalPrice > 0)
            {
                // If specific total price provided, use it
                newBooking.TotalPrice = bookingsDto.TotalPrice;
                newBooking.TicketQuantity = bookingsDto.TicketQuantity;
            }

            await _dbContext.Bookings.AddAsync(newBooking, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create and return response DTO
            var responseDto = new BookingsResponseDto
            {
                Id = newBooking.Id,
                UsersId = newBooking.UsersId,
                ShowTimesId = newBooking.ShowTimesId,
                BookingDate = newBooking.BookingDate,
                TicketQuantity = newBooking.TicketQuantity,
                TotalPrice = newBooking.TotalPrice,
                BookingStatus = newBooking.BookingStatus,
                BookedSeatsIds = newBooking.BookedSeats?.Select(bs => bs.SeatsId).ToList() ?? new List<int>()
            };

            return responseDto;
        }

        public async Task<bool> DeleteBookingsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.BookedSeats)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (booking != null)
            {
                if (booking.BookedSeats != null)
                {
                    _dbContext.BookedSeats.RemoveRange(booking.BookedSeats);
                }
                _dbContext.Bookings.Remove(booking);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<List<BookingsDto>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
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
                .ToListAsync(cancellationToken);

            return bookings;
        }

        public async Task<BookingsDto> GetBookingsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.ShowTimes)
                    .ThenInclude(s => s.Movies)
                .Include(b => b.ShowTimes)
                    .ThenInclude(s => s.Halls)
                .Include(b => b.BookedSeats)
                    .ThenInclude(bs => bs.Seats)
                .Where(b => b.Id == id)
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
                    BookedSeats = b.BookedSeats.Select(bs => bs.Seats.SeatNumber).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return booking;
        }
        public async Task<BookingsResponseDto> UpdateBookingsAsync(int id, BookingsEditDto bookingsDto, CancellationToken cancellationToken = default)
        {
            var existingBooking = await _dbContext.Bookings
                .Include(b => b.BookedSeats)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (existingBooking == null)
            {
                return null;
            }

            // Check if showtime exists if it's being changed
            if (existingBooking.ShowTimesId != bookingsDto.ShowTimesId)
            {
                var showTimes = await _dbContext.ShowTimes.FindAsync(new object[] { bookingsDto.ShowTimesId }, cancellationToken);
                if (showTimes == null)
                {
                    throw new InvalidOperationException($"ShowTime with ID {bookingsDto.ShowTimesId} does not exist.");
                }
                existingBooking.ShowTimesId = bookingsDto.ShowTimesId;
            }

            // Update basic booking info
            existingBooking.BookingStatus = bookingsDto.BookingStatus;

            // Handle nullable DateTime conversion
            existingBooking.BookingDate = bookingsDto.BookingDate ?? DateTime.Now; // Use current time as default if null

            // Handle booked seats changes
            if (bookingsDto.BookedSeatsIds != null && bookingsDto.BookedSeatsIds.Count > 0)
            {
                // Get the showtime to calculate the ticket price
                var showTimes = await _dbContext.ShowTimes.FindAsync(new object[] { existingBooking.ShowTimesId }, cancellationToken);

                // Remove existing booked seats
                if (existingBooking.BookedSeats != null)
                {
                    _dbContext.BookedSeats.RemoveRange(existingBooking.BookedSeats);
                }

                // Add new booked seats
                existingBooking.BookedSeats = new List<BookedSeats>();
                foreach (var seatId in bookingsDto.BookedSeatsIds)
                {
                    var seat = await _dbContext.Seats.FindAsync(new object[] { seatId }, cancellationToken);
                    if (seat == null)
                    {
                        throw new InvalidOperationException($"Seat {seatId} does not exist.");
                    }

                    // Check if seat is already booked by someone else
                    var existingBookedSeat = await _dbContext.BookedSeats
                        .FirstOrDefaultAsync(bs => bs.SeatsId == seatId &&
                                                  bs.Bookings.ShowTimesId == existingBooking.ShowTimesId &&
                                                  bs.Bookings.Id != existingBooking.Id,
                                            cancellationToken);

                    if (existingBookedSeat != null)
                    {
                        throw new InvalidOperationException($"Seat {seatId} is already booked for this showtime.");
                    }

                    // Create a new BookedSeats item - adjust property name as needed for your model
                    var newBookedSeat = new BookedSeats
                    {
                        SeatsId = seatId,
                        // Use the appropriate property name from your BookedSeats class
                        // This could be BookingId, BookingsId, or something else
                        Bookings = existingBooking  // Using navigation property if direct ID property isn't available
                    };

                    existingBooking.BookedSeats.Add(newBookedSeat);
                }

                // Update ticket quantity and total price
                existingBooking.TicketQuantity = existingBooking.BookedSeats.Count;
                existingBooking.TotalPrice = existingBooking.TicketQuantity * showTimes.TicketPrice;
            }
            else if (bookingsDto.TotalPrice > 0)
            {
                // If specific total price provided, use it
                existingBooking.TotalPrice = bookingsDto.TotalPrice;
                existingBooking.TicketQuantity = bookingsDto.TicketQuantity;
            }

            _dbContext.Bookings.Update(existingBooking);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create and return response DTO
            var responseDto = new BookingsResponseDto
            {
                Id = existingBooking.Id,
                UsersId = existingBooking.UsersId,
                ShowTimesId = existingBooking.ShowTimesId,
                BookingDate = existingBooking.BookingDate,
                TicketQuantity = existingBooking.TicketQuantity,
                TotalPrice = existingBooking.TotalPrice,
                BookingStatus = existingBooking.BookingStatus,
                BookedSeatsIds = existingBooking.BookedSeats?.Select(bs => bs.SeatsId).ToList() ?? new List<int>()
            };

            return responseDto;
        }
    }
}
