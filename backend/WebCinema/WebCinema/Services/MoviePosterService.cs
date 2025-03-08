using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;
using WebCinema.Models;
using System.Threading;
using System.Threading.Tasks;

namespace WebCinema.Services
{
    public class MoviePosterService : IMoviesImageService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<MoviePosterService> _logger;

        public MoviePosterService(WebCinemaDBContext dbContext, ILogger<MoviePosterService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateMoviePosterAsync(MovieCreatePosterDto posterDto, CancellationToken cancellationToken)
        {
            try
            {
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
                    .FirstOrDefaultAsync(x => x.MovieId == posterDto.MovieId, cancellationToken);
                if (existingPoster != null)
                {
                    _dbContext.MoviePoster.Remove(existingPoster);
                }

                await _dbContext.MoviePoster.AddAsync(moviePoster, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating movie poster");
                return false;
            }
        }

        public async Task<bool> DeleteMoviePosterByIdAsync(int imageId, CancellationToken cancellationToken)
        {
            try
            {
                var movieImage = await _dbContext.MoviePoster.FindAsync(new object[] { imageId }, cancellationToken);
                if (movieImage == null)
                {
                    _logger.LogWarning($"Image with ID {imageId} not found");
                    return false;
                }

                _dbContext.MoviePoster.Remove(movieImage);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting movie image");
                return false;
            }
        }

        public async Task<List<MoviePosterResponseDto>> GetAllMoviePostersAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.MoviePoster
                    .Select(mp => new MoviePosterResponseDto
                    {
                        Id = mp.Id,
                        Image = mp.ImageFormat + Convert.ToBase64String(mp.PosterImage),
                        ImageFormat = mp.ImageFormat
                    })
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all movie posters");
                return new List<MoviePosterResponseDto>();
            }
        }

        public async Task<MoviePosterResponseDto?> GetPosterByMovieIdAsync(int id, CancellationToken cancellationToken)
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
                    .FirstOrDefaultAsync(cancellationToken);

                return poster;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving poster for movie ID {id}");
                return null;
            }
        }

        public async Task<MoviePosterResponseDto?> GetMoviePosterByTitleAsync(string title, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return null;

                return await _dbContext.MoviePoster
                    .Include(mp => mp.Movies)
                    .Where(mp => mp.Movies.Title.Contains(title))
                    .Select(mp => new MoviePosterResponseDto
                    {
                        Id = mp.Id,
                        Image = mp.ImageFormat + Convert.ToBase64String(mp.PosterImage),
                        ImageFormat = mp.ImageFormat
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving poster for movie title '{title}'");
                return null;
            }
        }
    }
}
