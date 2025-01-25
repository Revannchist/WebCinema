using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesPostersController : ControllerBase
    {
        private readonly IMoviesImageService _moviesImageService;

        public MoviesPostersController(IMoviesImageService moviesImageService)
        {
            _moviesImageService = moviesImageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMoviePoster([FromBody] MovieCreatePosterDto imageDto)
        {
            var created = await _moviesImageService.CreateMoviePosterAsync(imageDto);
            if (!created)
            {
                return BadRequest("Error | Bad Request!");
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
