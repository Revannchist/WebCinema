using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;
using WebCinema.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebCinema.Services
{
    public class MoviesImageService : IMoviesImageService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<MoviesImageService> _logger;

        public MoviesImageService(WebCinemaDBContext dbcontext, ILogger<MoviesImageService> logger)
        {
            _dbContext = dbcontext;
            _logger = logger;
        }

        public async Task<bool> CreateMovieImageAsync(MoviesImageDto imageDto)
        {
            try
            {
                var movieExists = await _dbContext.Movies.AnyAsync(m => m.Id == imageDto.MovieId);
                if (!movieExists)
                {
                    _logger.LogWarning($"Movie with ID {imageDto.MovieId} not found");
                    return false;
                }

                if (string.IsNullOrEmpty(imageDto.Image))
                {
                    _logger.LogWarning("Image data is empty");
                    return false;
                }

                int commaIndex = imageDto.Image.IndexOf(',');
                var format = imageDto.Image.Substring(0, commaIndex + 1);
                var imageString = imageDto.Image.Substring(commaIndex + 1);

                var movieImage = new MoviesImage
                {
                    MovieId = imageDto.MovieId,
                    ImageByteArray = Convert.FromBase64String(imageString),
                    ImageFormat = format,
                    IsPoster = imageDto.IsPoster
                };

                //ako je IsPoster stavljeno true onda zamjeni stari sa novim posterom
                if (imageDto.IsPoster)
                {
                    var existingPoster = await _dbContext.MoviesImages
                        .FirstOrDefaultAsync(x => x.MovieId == imageDto.MovieId && x.IsPoster);

                    if (existingPoster != null)
                    {
                        _dbContext.MoviesImages.Remove(existingPoster);
                    }
                }
                //ako je  IsPoster stavljeno na false onda samo doda jos jednu sliku u niz 

                await _dbContext.MoviesImages.AddAsync(movieImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating movie image");
                return false;
            }
        }

        public async Task<bool> DeleteMovieImageByIdAsync(int imageId)
        {
            try
            {
                var movieImage = await _dbContext.MoviesImages.FindAsync(imageId);
                if (movieImage == null)
                {
                    _logger.LogWarning($"Image with ID {imageId} not found");
                    return false;
                }

                _dbContext.MoviesImages.Remove(movieImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting movie image");
                return false;
            }
        }

        public async Task<List<MoviesImageDto>> GetAllMovieImagesAsync()
        {
            try
            {
                var movieImages = await _dbContext.MoviesImages
                    .Select(mi => new MoviesImageDto
                    {
                        Id = mi.Id,
                        MovieId = mi.MovieId,
                        Image = mi.ImageFormat + Convert.ToBase64String(mi.ImageByteArray),
                        IsPoster = mi.IsPoster
                    })
                    .ToListAsync();

                return movieImages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all movie images");
                return new List<MoviesImageDto>();
            }
        }

        public async Task<List<MoviesImageDto>> GetImagesByMovieIdAsync(int movieId)
        {
            try
            {
                var movieImages = await _dbContext.MoviesImages
                    .Where(mi => mi.MovieId == movieId)
                    .Select(mi => new MoviesImageDto
                    {
                        Id = mi.Id,
                        MovieId = mi.MovieId,
                        Image = mi.ImageFormat + Convert.ToBase64String(mi.ImageByteArray),
                        IsPoster = mi.IsPoster
                    })
                    .ToListAsync();

                return movieImages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving images for movie ID {movieId}");
                return new List<MoviesImageDto>();
            }
        }

        public async Task<List<MoviesImageDto>> GetMovieImagesByMovieTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return new List<MoviesImageDto>();

                var movieImages = await _dbContext.MoviesImages
                    .Include(mi => mi.Movies)
                    .Where(mi => mi.Movies.Title.Contains(title))
                    .Select(mi => new MoviesImageDto
                    {
                        Id = mi.Id,
                        MovieId = mi.MovieId,
                        Image = mi.ImageFormat + Convert.ToBase64String(mi.ImageByteArray),
                        IsPoster = mi.IsPoster
                    })
                    .ToListAsync();

                return movieImages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving images for movie title '{title}'");
                return new List<MoviesImageDto>();
            }
        }

    }
}
