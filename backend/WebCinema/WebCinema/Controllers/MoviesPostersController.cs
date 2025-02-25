using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesPostersController : ControllerBase
    {
        private readonly IMoviesImageService _moviesImageService;
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<MoviePosterService> _logger;


        public MoviesPostersController(IMoviesImageService moviesImageService, WebCinemaDBContext dbcontext, ILogger<MoviePosterService> logger)
        {
            _moviesImageService = moviesImageService;
            _dbContext = dbcontext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddMoviePoster([FromBody] MovieCreatePosterDto imageDto)
        {
            // Validate the movie exists
            var movieExists = await _dbContext.Movies.AnyAsync(m => m.Id == imageDto.MovieId);
            if (!movieExists)
            {
                _logger.LogWarning($"Movie with ID {imageDto.MovieId} not found");
                return BadRequest("Movie not found");
            }

            // Validate image data
            if (string.IsNullOrEmpty(imageDto.Image))
            {
                _logger.LogWarning("Image data is empty");
                return BadRequest("Image data is required");
            }

            if (!imageDto.Image.StartsWith("data:image/"))
            {
                _logger.LogWarning("Invalid image format: not an image data URI");
                return BadRequest("Invalid image format: not a valid data URI");
            }

            if (!imageDto.Image.StartsWith("data:image/jpeg") &&
                !imageDto.Image.StartsWith("data:image/png"))
            {
                _logger.LogWarning("Unsupported image format. Only JPEG and PNG are allowed");
                return BadRequest("Unsupported image format. Only JPEG and PNG are allowed");
            }

            int commaIndex = imageDto.Image.IndexOf(',');
            if (commaIndex < 0)
            {
                _logger.LogWarning("Invalid image format: missing data URI comma separator");
                return BadRequest("Invalid image format: missing data URI comma separator");
            }

            var imageString = imageDto.Image.Substring(commaIndex + 1);
            if (string.IsNullOrEmpty(imageString))
            {
                _logger.LogWarning("Image data is empty");
                return BadRequest("Image data is empty");
            }

            try
            {
                var imageBytes = Convert.FromBase64String(imageString);
                if (imageBytes.Length == 0)
                {
                    _logger.LogWarning("Image has zero bytes");
                    return BadRequest("Image has zero bytes");
                }
            }
            catch (FormatException)
            {
                _logger.LogWarning("Invalid base64 string");
                return BadRequest("Invalid base64 string");
            }

            // If all validations pass, create the poster
            var created = await _moviesImageService.CreateMoviePosterAsync(imageDto);
            if (!created)
            {
                return BadRequest("Error creating movie poster");
            }

            return Ok(created);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMoviePosterById(int imageId)
        {
            var deleted = await _moviesImageService.DeleteMoviePosterByIdAsync(imageId);
            if (!deleted)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMoviePosters()
        {
            var images = await _moviesImageService.GetAllMoviePostersAsync();
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }

        [HttpGet]
        public async Task<ActionResult<MoviePosterResponseDto>> GetPosterByMovieId(/*[FromQuery]*/ int movieId)
        {
            var poster = await _moviesImageService.GetPosterByMovieIdAsync(movieId);
            if (poster == null)
            {
                return NotFound($"No poster found for movie ID: {movieId}"); 
            }
            return Ok(poster);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviePosterByMovieTitle(string title)
        {
            var images = await _moviesImageService.GetMoviePosterByTitleAsync(title);
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }


    }
}
