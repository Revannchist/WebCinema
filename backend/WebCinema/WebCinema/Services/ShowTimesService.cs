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
            var showtimes = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id);
            if (showtimes != null)
            {
                _dbContext.ShowTimes.Remove(showtimes);
                await _dbContext.SaveChangesAsync();
            }
            return showtimes;
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
                    ShowDateTieme = s.ShowDateTieme,
                    TicketPrice = s.TicketPrice
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
                    ShowDateTieme = s.ShowDateTieme,
                    TicketPrice = s.TicketPrice
                })
                .FirstOrDefaultAsync();

            return showtime;
        }

        public async Task<ShowTimes> UpdateShowTimesAsync(int id, ShowTimes showtimes)
        {
            var existingShowtime = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingShowtime != null)
            {
                existingShowtime.ShowDateTieme = showtimes.ShowDateTieme;
                existingShowtime.TicketPrice = showtimes.TicketPrice;
                _dbContext.ShowTimes.Update(existingShowtime);
                await _dbContext.SaveChangesAsync();
            }
            return existingShowtime;
        }
    }
}
