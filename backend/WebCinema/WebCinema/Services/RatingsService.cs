using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class RatingsService : IRatingsService
    {

        private readonly WebCinemaDBContext _dbContext;
        public RatingsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Ratings> CreateRatingsAsync(Ratings ratings)
        {
            if (ratings == null)
            {
                return null;
            }
            if (ratings.Rating < 1 || ratings.Rating > 5)
            {
                throw new ArgumentException("Ocjena mora biti između 1 i 5");
            }
            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r =>
                r.UsersId == ratings.UsersId &&
                r.MoviesId == ratings.MoviesId);

            if (existingRating != null)
            {
                throw new Exception("Vec ste ocijenili ovaj film!");
            }
            await _dbContext.Ratings.AddAsync(ratings);
            await _dbContext.SaveChangesAsync();
            return ratings;
        }

        public async Task<Ratings> DeleteRatingsByIdAsync(int id)
        {
            var ratings = await GetRatingsByIdAsync(id);
            if (ratings != null)
            {
                _dbContext.Ratings.Remove(ratings);
                await _dbContext.SaveChangesAsync();
            }
            return ratings;
        }

        public async Task<List<Ratings>> GetAllRatingsAsync()
        {
            var ratings = await _dbContext.Ratings.ToListAsync();

            return ratings;
        }

        public async Task<Ratings> GetRatingsByIdAsync(int id)
        {
            var ratings = await _dbContext.Ratings.FirstOrDefaultAsync(x => x.Id == id);
            return ratings;
        }

        public async Task<Ratings> UpdateRatingsAsync(int id, Ratings ratings)
        {
            var _ratings = await GetRatingsByIdAsync(id);
            if (ratings != null)
            {
                _ratings.Rating=ratings.Rating;
                _ratings.Review=ratings.Review;
                _ratings.RatingDateTime=ratings.RatingDateTime;
                _dbContext.Ratings.Update(_ratings);
                await _dbContext.SaveChangesAsync();
            }
            return _ratings;
        }
    }
}
