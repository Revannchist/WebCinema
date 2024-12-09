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
        public async Task<IActionResult> AddMovie(Movies movie)
        {
            var createdMovies = await _moviesService.CreateMovieAsync(movie);
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
        public async Task<IActionResult> AddGenreToMovie(int genreId, int movieId)
        {

            var movie = await _moviesService.AddGenreToMovieAsync(genreId, movieId);
            if(movie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddActorToMovie(int actorId, int movieId)
        {

            var actor = await _moviesService.AddActorToMovieAsync(actorId, movieId);
            if (actor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovieGenre(int genreId, int movieId, Genres genre)
        {
            var updatedMovie = await _moviesService.UpdateMovieGenreAsync(genreId, movieId, genre);
            if (updatedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedMovie);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovieActor(int actorId, int movieId, Actors actor)
        {
            var updatedActor = await _moviesService.UpdateMovieActorAsync(actorId, movieId, actor);
            if (updatedActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedActor);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMovie(int id, Movies movie)
        {
            var updatedMovie = await _moviesService.UpdateMovieAsync(id, movie);
            if (updatedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedMovie);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovieBasicInfo(int id, MoviesEditDTO dto)
        {
            var updatedMovie = await _moviesService.UpdateMovieBasicInfoAsync(id, dto);
            if (updatedMovie == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedMovie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _moviesService.GetAllMoviesAsync();
            if (movies == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(movies);
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
