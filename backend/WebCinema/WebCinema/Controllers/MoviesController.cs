using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMoviesService moviesService, ILogger<MoviesController> logger)
        {
            _moviesService = moviesService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto movieDto, CancellationToken cancellationToken)
        {
            try
            {
                var createdMovies = await _moviesService.CreateMovieAsync(movieDto, cancellationToken);
                if (createdMovies == null)
                {
                    return BadRequest("Error creating movie!");
                }
                return Ok(createdMovies);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Create movie operation was canceled");
                return StatusCode(499, "Request canceled");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteMovieById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deletedMovie = await _moviesService.DeleteMovieByIdAsync(id, cancellationToken);
                if (deletedMovie == null)
                {
                    return NotFound($"Movie with ID {id} not found");
                }
                return Ok(deletedMovie);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Delete movie operation was canceled for ID: {MovieId}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateMovie(int id, MoviesUpdateDto movieDto, CancellationToken cancellationToken)
        {
            try
            {
                var updatedMovie = await _moviesService.UpdateMovieAsync(id, movieDto, cancellationToken);
                if (updatedMovie == null)
                {
                    return NotFound($"Movie with ID {id} not found");
                }
                return Ok(updatedMovie);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Update movie operation was canceled for ID: {MovieId}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateMovieBasicInfo(int id, MoviesUpdateBasicDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updatedMovie = await _moviesService.UpdateMovieBasicInfoAsync(id, dto, cancellationToken);
                if (updatedMovie == null)
                {
                    return NotFound($"Movie with ID {id} not found");
                }
                return Ok(updatedMovie);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Update movie basic info operation was canceled for ID: {MovieId}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies([FromQuery] MoviesParameters parameters, CancellationToken cancellationToken)
        {
            try
            {
                var movies = await _moviesService.GetAllMoviesAsync(parameters, cancellationToken);

                if (movies.Items == null || !movies.Items.Any())
                {
                    return NotFound("No movies found matching the criteria.");
                }

                return Ok(movies);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get all movies operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMovieById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _moviesService.GetMovieByIdAsync(id, cancellationToken);
                if (movie == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(movie);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get movie operation was canceled for ID: {MovieId}", id);
                return StatusCode(499, "Request canceled");
            }
        }
    }
}