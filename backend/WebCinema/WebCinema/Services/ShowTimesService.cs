using Microsoft.EntityFrameworkCore;
using System.Threading;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class ShowTimesService : IShowTimesService
    {
        private readonly WebCinemaDBContext _dbContext;
        public ShowTimesService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShowTimes> CreateShowTimesAsync(ShowTimes showtimes, CancellationToken cancellationToken = default)
        {
            if (showtimes == null)
            {
                return null;
            }
            await _dbContext.ShowTimes.AddAsync(showtimes, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return showtimes;
        }

        public async Task<ShowTimes> DeleteShowTimesByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var showtime = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (showtime != null)
            {
                var relatedBookings = await _dbContext.Bookings
                    .Where(b => b.ShowTimesId == id)
                    .ToListAsync(cancellationToken);

                foreach (var booking in relatedBookings)
                {
                    var bookedSeats = await _dbContext.BookedSeats
                        .Where(bs => bs.BookingId == booking.Id)
                        .ToListAsync(cancellationToken);

                    _dbContext.BookedSeats.RemoveRange(bookedSeats);
                }

                _dbContext.Bookings.RemoveRange(relatedBookings);

                _dbContext.ShowTimes.Remove(showtime);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return showtime;
        }

        public async Task<List<ShowTimesDto>> GetAllShowTimesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var showtimes = await _dbContext.ShowTimes
                .Include(s => s.Movies)
                .Include(s => s.Halls)
                .ToListAsync(cancellationToken);

            bool changesMade = false;
            foreach (var showtime in showtimes)
            {
                if (showtime.ShowDateTime < now && showtime.IsActive)
                {
                    showtime.IsActive = false;
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return showtimes.Select(s => new ShowTimesDto
            {
                Id = s.Id,
                MoviesId = s.MoviesId,
                MovieTitle = s.Movies.Title,
                HallsId = s.HallsId,
                HallName = s.Halls.HallName,
                ShowDateTime = s.ShowDateTime,
                TicketPrice = s.TicketPrice,
                IsActive = s.IsActive
            }).ToList();
        }


        public async Task<ShowTimesDto> GetShowTimesByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var showtime = await _dbContext.ShowTimes
                .Include(s => s.Movies)
                .Include(s => s.Halls)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (showtime == null)
                return null;

            if (showtime.ShowDateTime < now && showtime.IsActive)
            {
                showtime.IsActive = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new ShowTimesDto
            {
                Id = showtime.Id,
                MoviesId = showtime.MoviesId,
                MovieTitle = showtime.Movies.Title,
                HallsId = showtime.HallsId,
                HallName = showtime.Halls.HallName,
                ShowDateTime = showtime.ShowDateTime,
                TicketPrice = showtime.TicketPrice,
                IsActive = showtime.IsActive
            };
        }


        public async Task<ShowTimesDto?> UpdateShowTimesAsync(int id, ShowTimesUpdateDto updateDto, CancellationToken cancellationToken = default)
        {
            var existingShowtime = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (existingShowtime == null)
            {
                return null; // Handle not found case
            }

            // Update only the allowed fields
            existingShowtime.MoviesId = updateDto.MoviesId;
            existingShowtime.HallsId = updateDto.HallsId;
            existingShowtime.ShowDateTime = updateDto.ShowDateTime;
            existingShowtime.TicketPrice = updateDto.TicketPrice;
            existingShowtime.IsActive = updateDto.IsActive;

            // Save changes
            _dbContext.ShowTimes.Update(existingShowtime);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return updated data as ShowTimesDto
            return new ShowTimesDto
            {
                Id = existingShowtime.Id,
                MoviesId = existingShowtime.MoviesId,
                MovieTitle = existingShowtime.Movies?.Title ?? string.Empty,
                HallsId = existingShowtime.HallsId,
                HallName = existingShowtime.Halls?.HallName ?? string.Empty,
                ShowDateTime = existingShowtime.ShowDateTime,
                TicketPrice = existingShowtime.TicketPrice,
                IsActive = existingShowtime.IsActive
            };
        }
    }
}