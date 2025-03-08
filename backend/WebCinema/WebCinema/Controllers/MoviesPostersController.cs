using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesPostersController : ControllerBase
    {
        private readonly IMoviesImageService _moviesImageService;
        private readonly ILogger<MoviesPostersController> _logger;

        public MoviesPostersController(IMoviesImageService moviesImageService, ILogger<MoviesPostersController> logger)
        {
            _moviesImageService = moviesImageService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMoviePoster([FromBody] MovieCreatePosterDto imageDto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _moviesImageService.CreateMoviePosterAsync(imageDto, cancellationToken);
                if (!created)
                {
                    return BadRequest("Error creating movie poster");
                }
                return Ok(created);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Add movie poster operation was canceled");
                return StatusCode(499, "Request canceled");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteMoviePosterById(int imageId, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _moviesImageService.DeleteMoviePosterByIdAsync(imageId, cancellationToken);
                if (!deleted)
                {
                    return NotFound($"Movie poster with ID {imageId} not found");
                }
                return Ok(deleted);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Delete movie poster operation was canceled for ID: {ImageId}", imageId);
                return StatusCode(499, "Request canceled");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMoviePosters(CancellationToken cancellationToken)
        {
            try
            {
                var posters = await _moviesImageService.GetAllMoviePostersAsync(cancellationToken);
                return Ok(posters);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get all movie posters operation was canceled");
                return StatusCode(499, "Request canceled");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPosterByMovieId(int movieId, CancellationToken cancellationToken)
        {
            try
            {
                var poster = await _moviesImageService.GetPosterByMovieIdAsync(movieId, cancellationToken);
                if (poster == null)
                {
                    return NotFound($"No poster found for movie ID: {movieId}");
                }
                return Ok(poster);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get poster by movie ID operation was canceled for ID: {MovieId}", movieId);
                return StatusCode(499, "Request canceled");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviePosterByMovieTitle(string title, CancellationToken cancellationToken)
        {
            try
            {
                var poster = await _moviesImageService.GetMoviePosterByTitleAsync(title, cancellationToken);
                if (poster == null)
                {
                    return NotFound($"No poster found for movie title: {title}");
                }
                return Ok(poster);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get poster by movie title operation was canceled for title: {Title}", title);
                return StatusCode(499, "Request canceled");
            }
        }
    }
}
