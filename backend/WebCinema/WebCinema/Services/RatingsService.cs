using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class RatingsService : IRatingsService
    {

        private readonly WebCinemaDBContext _dbContext;
        public RatingsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RatingsResponseDto> CreateRatingAsync(RatingCreateDto ratingDto)
        {
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

            // Load related data for response
            await _dbContext.Entry(rating)
                .Reference(r => r.Movies)
                .LoadAsync();

            await _dbContext.Entry(rating)
                .Reference(r => r.Users)
                .LoadAsync();

            // Load related movie data and user data
            var movieResponse = new MoviesResponseDto
            {
                Id = rating.Movies.Id,
                Title = rating.Movies.Title,
                Description = rating.Movies.Description,
                ReleaseDate = rating.Movies.ReleaseDate,
                Duration = rating.Movies.Duration,
                Language = rating.Movies.Language,
                AgeRating = rating.Movies.AgeRating,
                Director = rating.Movies.Director != null ? new DirectorDto
                {
                    Id = rating.Movies.Director.Id,
                    FirstName = rating.Movies.Director.FirstName,
                    LastName = rating.Movies.Director.LastName
                } : null,

                Country = rating.Movies.Country != null ? new CountryDto
                {
                    Id = rating.Movies.Country.Id,
                    Name = rating.Movies.Country.Name
                } : null,

                Genres = rating.Movies.MoviesGenres?.Select(mg => new GenreDto
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                }).ToList(),

                Actors = rating.Movies.MoviesActors?.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    FirstName = ma.Actor.FirstName,
                    LastName = ma.Actor.LastName
                }).ToList()
            };

            return new RatingsResponseDto
            {
                Id = rating.Id,
                MoviesId = rating.MoviesId,
                UsersId = rating.UsersId,
                Rating = rating.Rating,
                Review = rating.Review,
                RatingDateTime = rating.RatingDateTime,
                //User = rating.Users != null ? new UserDto
                //{
                //    Id = rating.Users.Id,
                //    FirstName = rating.Users.FirstName,
                //    LastName = rating.Users.LastName
                //} : null,

                Movie = movieResponse  // Assigning MovieResponseDto
            };
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
