using Microsoft.AspNetCore.Mvc;
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

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto movieDto)
        {
            var createdMovies = await _moviesService.CreateMovieAsync(movieDto);
            if (createdMovies == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdMovies);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovieById(int id)
        {
            var deletedMovie = await _moviesService.DeleteMovieByIdAsync(id);
            if (deletedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedMovie);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(int id, MoviesUpdateDto movieDto)
        {
            var updatedMovie = await _moviesService.UpdateMovieAsync(id, movieDto);
            if (updatedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedMovie);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovieBasicInfo(int id, MoviesUpdateBasicDto dto)
        {
            var updatedMovie = await _moviesService.UpdateMovieBasicInfoAsync(id, dto);
            if (updatedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedMovie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies([FromQuery] MoviesParameters parameters)
        {
            try
            {
                var movies = await _moviesService.GetAllMoviesAsync(parameters);

                if (movies.Items == null || !movies.Items.Any())
                {
                    return NotFound("No movies found matching the criteria.");
                }

                return Ok(movies);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _moviesService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(movie);
        }
    }
}
