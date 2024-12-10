using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

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
            var showtimes = await GetShowTimesByIdAsync(id);
            if (showtimes != null)
            {
                _dbContext.ShowTimes.Remove(showtimes);
                await _dbContext.SaveChangesAsync();
            }
            return showtimes;
        }

        public async Task<List<ShowTimes>> GetAllShowTimesAsync()
        {
            var showtimes = await _dbContext.ShowTimes.ToListAsync();

            return showtimes;
        }

        public async Task<ShowTimes> GetShowTimesByIdAsync(int id)
        {
            var showtimes = await _dbContext.ShowTimes.FirstOrDefaultAsync(x => x.Id == id);
            return showtimes;
        }

        public async Task<ShowTimes> UpdateShowTimesAsync(int id, ShowTimes showtimes)
        {
            var _showtimes = await GetShowTimesByIdAsync(id);
            if (showtimes != null)
            {
                
                _showtimes.ShowDateTieme = showtimes.ShowDateTieme;
                _showtimes.TicketPrice = showtimes.TicketPrice;
                _dbContext.ShowTimes.Update(_showtimes);
                await _dbContext.SaveChangesAsync();
            }
            return _showtimes;
        }
    }
}
