using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;
using WebCinema.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebCinema.Services
{
    public class MoviePosterService : IMoviesImageService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<MoviePosterService> _logger;

        public MoviePosterService(WebCinemaDBContext dbcontext, ILogger<MoviePosterService> logger)
        {
            _dbContext = dbcontext;
            _logger = logger;
        }


        // Service method
        //public async Task<bool> CreateMoviePosterAsync(MovieCreatePosterDto posterDto)
        //{
        //    try
        //    {
        //        var movieExists = await _dbContext.Movies.AnyAsync(m => m.Id == posterDto.MovieId);
        //        if (!movieExists)
        //        {
        //            _logger.LogWarning($"Movie with ID {posterDto.MovieId} not found");
        //            return false;
        //        }

        //        if (string.IsNullOrEmpty(posterDto.Image))
        //        {
        //            _logger.LogWarning("Image data is empty");
        //            return false;
        //        }

        //        // Create new poster and extract format/image data all at once
        //        var moviePoster = new MoviePoster
        //        {
        //            MovieId = posterDto.MovieId,
        //            PosterImage = Convert.FromBase64String(posterDto.Image),
        //            ImageFormat = posterDto.Image.Substring(0, posterDto.Image.IndexOf(",") + 1)
        //        };

        //        // Check for existing poster and remove it
        //        var existingPoster = await _dbContext.MoviePoster
        //            .FirstOrDefaultAsync(x => x.MovieId == posterDto.MovieId);

        //        if (existingPoster != null)
        //        {
        //            _dbContext.MoviePoster.Remove(existingPoster);
        //        }

        //        await _dbContext.MoviePoster.AddAsync(moviePoster);
        //        await _dbContext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating movie poster");
        //        return false;
        //    }
        //}


        public async Task<bool> CreateMoviePosterAsync(MovieCreatePosterDto posterDto)
        {
            try
            {
                var movieExists = await _dbContext.Movies.AnyAsync(m => m.Id == posterDto.MovieId);
                if (!movieExists)
                {
                    _logger.LogWarning($"Movie with ID {posterDto.MovieId} not found");
                    return false;
                }

                if (string.IsNullOrEmpty(posterDto.Image))
                {
                    _logger.LogWarning("Image data is empty");
                    return false;
                }

                int commaIndex = posterDto.Image.IndexOf(',');
                var format = posterDto.Image.Substring(0, commaIndex + 1);
                var imageString = posterDto.Image.Substring(commaIndex + 1);

                var moviePoster = new MoviePoster
                {
                    MovieId = posterDto.MovieId,
                    PosterImage = Convert.FromBase64String(imageString),
                    ImageFormat = format
                };

                var existingPoster = await _dbContext.MoviePoster
                    .FirstOrDefaultAsync(x => x.MovieId == posterDto.MovieId);

                if (existingPoster != null)
                {
                    _dbContext.MoviePoster.Remove(existingPoster);
                }

                await _dbContext.MoviePoster.AddAsync(moviePoster);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating movie poster");
                return false;
            }
        }

        public async Task<bool> DeleteMoviePosterByIdAsync(int imageId)
        {
            try
            {
                var movieImage = await _dbContext.MoviePoster.FindAsync(imageId);
                if (movieImage == null)
                {
                    _logger.LogWarning($"Image with ID {imageId} not found");
                    return false;
                }

                _dbContext.MoviePoster.Remove(movieImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting movie image");
                return false;
            }
        }

        public async Task<List<MoviePosterResponseDto>> GetAllMoviePostersAsync()
        {
            try
            {
                var moviePosters = await _dbContext.MoviePoster
                    .Select(mp => new MoviePosterResponseDto
                    {
                        Id = mp.Id,
                        Image = mp.ImageFormat + Convert.ToBase64String(mp.PosterImage),
                        ImageFormat = mp.ImageFormat
                    })
                    .ToListAsync();

                return moviePosters;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all movie posters");
                return new List<MoviePosterResponseDto>();
            }
        }

        public async Task<MoviePosterResponseDto?> GetPosterByMovieIdAsync(int id)
        {
            try
            {
                var poster = await _dbContext.MoviePoster
                    .Where(mp => mp.MovieId == id)
                    .Select(mp => new MoviePosterResponseDto
                    {
                        Id = mp.Id,
                        Image = mp.ImageFormat + Convert.ToBase64String(mp.PosterImage),
                        ImageFormat = mp.ImageFormat
                    })
                    .FirstOrDefaultAsync();

                _logger.LogInformation($"Poster format: {poster?.ImageFormat}");
                _logger.LogInformation($"Image data length: {poster?.Image?.Length}");

                return poster;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving poster for movie ID {id}");
                return null;
            }
        }

        public async Task<MoviePosterResponseDto?> GetMoviePosterByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return null;

                var poster = await _dbContext.MoviePoster
                    .Include(mp => mp.Movies)
                    .Where(mp => mp.Movies.Title.Contains(title))
                    .Select(mp => new MoviePosterResponseDto
                    {
                        Id = mp.Id,
                        Image = mp.ImageFormat + Convert.ToBase64String(mp.PosterImage),
                        ImageFormat = mp.ImageFormat
                    })
                    .FirstOrDefaultAsync();

                return poster;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving poster for movie title '{title}'");
                return null;
            }
        }

    }
}
