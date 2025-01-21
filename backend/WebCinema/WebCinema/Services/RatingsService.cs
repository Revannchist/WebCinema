using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class RatingsService : IRatingsService
    {

        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<RatingsService> _logger;
        public RatingsService(WebCinemaDBContext dbContext, ILogger<RatingsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }



        public async Task<RatingsResponseDto> CreateRatingAsync(RatingCreateDto ratingDto)
        {

            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.MoviesId == ratingDto.MoviesId && r.UsersId == ratingDto.UsersId);

            if (existingRating != null)
            {
                throw new InvalidOperationException("This user has already rated the movie.");
            }

            var rating = new Ratings
            {
                MoviesId = ratingDto.MoviesId,
                UsersId = ratingDto.UsersId,
                Rating = ratingDto.Rating,
                Review = ratingDto.Review,
                RatingDateTime = DateTime.UtcNow
            };

            await _dbContext.Ratings.AddAsync(rating);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(rating)
                .Reference(r => r.Movies)
                .LoadAsync();

            await _dbContext.Entry(rating)
                .Reference(r => r.Users)
                .LoadAsync();

            return new RatingsResponseDto
            {
                Id = rating.Id,
                MoviesId = rating.MoviesId,
                UsersId = rating.UsersId,
                Rating = rating.Rating,
                Review = rating.Review,
                RatingDateTime = rating.RatingDateTime,

                User = rating.Users != null ? new UsersDto
                {
                    Id = rating.Users.Id,
                    Username = rating.Users.Username,
                } : null,

                Movie = rating.Movies != null ? new MoviesResponseDto //mogu uklonit ova 2 responsa jer ovaj info nije bitan 
                {
                    Id = rating.Movies.Id,
                    Title = rating.Movies.Title,
                    Description = rating.Movies.Description,
                    ReleaseDate = rating.Movies.ReleaseDate,
                    Duration = rating.Movies.Duration,
                    AgeRating = rating.Movies.AgeRating
                } : null
            };
        }

        public async Task<Ratings?> DeleteRatingByIdAsync(int id)
        {
            try
            {
                var rating = await _dbContext.Ratings.FindAsync(id);

                if (rating == null)
                {
                    _logger.LogWarning($"Attempt to delete non-existent rating: {id}");
                    return null;
                }
                _dbContext.Ratings.Remove(rating);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Rating deleted successfully: {id}");
                return rating;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting rating with ID: {id}");
                throw;
            }
        }

        public async Task<List<RatingsGetDto>> GetAllRatingsAsync()
        {
            try
            {
                var ratings = await _dbContext.Ratings
                    .AsNoTracking()
                    .Include(r => r.Movies) // Include the related Movies
                    .Include(r => r.Users)  // Include the related Users
                    .Select(r => new RatingsGetDto
                    {
                        Id = r.Id,
                        MoviesId = r.MoviesId,
                        UsersId = r.UsersId,
                        Rating = r.Rating,
                        Review = r.Review,
                        RatingDateTime = r.RatingDateTime,

                        // Mapping related Movie information
                        MovieTitle = r.Movies.Title,
                        MovieDescription = r.Movies.Description,

                        // Mapping related User information
                        Username = r.Users.Username,
                        UserEmail = r.Users.Email
                    })
                    .ToListAsync();

                return ratings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ratings");
                throw;
            }
        }

        public async Task<RatingsGetDto> GetRatingsByIdAsync(int id)
        {
            try
            {
                var rating = await _dbContext.Ratings
                    .AsNoTracking()
                    .Where(r => r.Id == id)
                    .Select(r => new RatingsGetDto
                    {
                        Id = r.Id,
                        MoviesId = r.MoviesId,
                        UsersId = r.UsersId,
                        Rating = r.Rating,
                        Review = r.Review,
                        RatingDateTime = r.RatingDateTime,
                        MovieTitle = r.Movies.Title,
                        MovieDescription = r.Movies.Description,
                        Username = r.Users.Username,
                        UserEmail = r.Users.Email
                    })
                    .FirstOrDefaultAsync();

                if (rating == null)
                {
                    throw new KeyNotFoundException($"Rating with ID {id} not found");
                }

                return rating;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error retrieving rating with ID {RatingId}", id);
                throw;
            }
        }

        public async Task<RatingsResponseDto> UpdateRatingsAsync(int id, RatingUpdateDto ratingDto)
        {
            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRating == null)
            {
                _logger.LogWarning($"Rating not found for update: {id}");
                throw new InvalidOperationException($"Rating with ID {id} not found");
            }

            // Update properties
            existingRating.Rating = ratingDto.Rating;
            existingRating.Review = ratingDto.Review;
            existingRating.RatingDateTime = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Rating updated successfully: {id}");

            await _dbContext.Entry(existingRating)
                .Reference(r => r.Movies)
                .LoadAsync();

            await _dbContext.Entry(existingRating)
                .Reference(r => r.Users)
                .LoadAsync();

            return new RatingsResponseDto
            {
                Id = existingRating.Id,
                MoviesId = existingRating.MoviesId,
                UsersId = existingRating.UsersId,
                Rating = existingRating.Rating,
                Review = existingRating.Review,
                RatingDateTime = existingRating.RatingDateTime,

                User = existingRating.Users != null ? new UsersDto
                {
                    Id = existingRating.Users.Id,
                    FirstName = existingRating.Users.FirstName,
                    LastName = existingRating.Users.LastName,
                    Email = existingRating.Users.Email
                } : null,

                Movie = existingRating.Movies != null ? new MoviesResponseDto
                {
                    Id = existingRating.Movies.Id,
                    Title = existingRating.Movies.Title,
                    Description = existingRating.Movies.Description,
                    ReleaseDate = existingRating.Movies.ReleaseDate,
                    Duration = existingRating.Movies.Duration,
                    Language = existingRating.Movies.Language,
                    AgeRating = existingRating.Movies.AgeRating
                } : null
            };
        }

    }
}
