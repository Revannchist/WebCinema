using Microsoft.EntityFrameworkCore;
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

        public async Task<ShowTimes> CreateShowTimesAsync(ShowTimes showtimes)
        {
            if (showtimes == null)
            {
                return null;
            }
            await _dbContext.ShowTimes.AddAsync(showtimes);
            await _dbContext.SaveChangesAsync();
            return showtimes;
        }

        public async Task<ShowTimes> DeleteShowTimesByIdAsync(int id)
        {
            var showtime = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id);
            if (showtime != null)
            {
                var relatedBookings = await _dbContext.Bookings
                    .Where(b => b.ShowTimesId == id)
                    .ToListAsync();

                foreach (var booking in relatedBookings)
                {
                    var bookedSeats = await _dbContext.BookedSeats
                        .Where(bs => bs.BookingId == booking.Id)
                        .ToListAsync();

                    _dbContext.BookedSeats.RemoveRange(bookedSeats);
                }

                _dbContext.Bookings.RemoveRange(relatedBookings);

                _dbContext.ShowTimes.Remove(showtime);

                await _dbContext.SaveChangesAsync();
            }
            return showtime;
        }

        public async Task<List<ShowTimesDto>> GetAllShowTimesAsync()
        {
            var showtimes = await _dbContext.ShowTimes
                .Include(s => s.Movies)
                .Include(s => s.Halls)
                .Select(s => new ShowTimesDto
                {
                    Id = s.Id,
                    MoviesId = s.MoviesId,
                    MovieTitle = s.Movies.Title,
                    HallsId = s.HallsId,
                    HallName = s.Halls.HallName,
                    ShowDateTime = s.ShowDateTime,
                    TicketPrice = s.TicketPrice,
                    IsActive = s.IsActive,
                })
                .ToListAsync();

            return showtimes;
        }

        public async Task<ShowTimesDto> GetShowTimesByIdAsync(int id)
        {
            var showtime = await _dbContext.ShowTimes
                .Include(s => s.Movies)
                .Include(s => s.Halls)
                .Where(x => x.Id == id)
                .Select(s => new ShowTimesDto
                {
                    Id = s.Id,
                    MoviesId = s.MoviesId,
                    MovieTitle = s.Movies.Title,
                    HallsId = s.HallsId,
                    HallName = s.Halls.HallName,
                    ShowDateTime = s.ShowDateTime,
                    TicketPrice = s.TicketPrice,
                    IsActive = s.IsActive,
                })
                .FirstOrDefaultAsync();

            return showtime;
        }

        public async Task<ShowTimesDto?> UpdateShowTimesAsync(int id, ShowTimesUpdateDto updateDto)
        {
            var existingShowtime = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id);
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
            await _dbContext.SaveChangesAsync();

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
