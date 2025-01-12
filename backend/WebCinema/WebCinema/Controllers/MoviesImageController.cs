using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesImageController : ControllerBase
    {
        private readonly IMoviesImageService _moviesImageService;

        public MoviesImageController(IMoviesImageService moviesImageService)
        {
            _moviesImageService = moviesImageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMovieImage([FromBody] MoviesImageDto imageDto)
        {
            var created = await _moviesImageService.CreateMovieImageAsync(imageDto);
            if (!created)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(created);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovieImageById(int imageId)
        {
            var deleted = await _moviesImageService.DeleteMovieImageByIdAsync(imageId);
            if (!deleted)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieImages()
        {
            var images = await _moviesImageService.GetAllMovieImagesAsync();
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }

        [HttpGet]
        public async Task<IActionResult> GetImagesByMovieId(int movieId)
        {
            var images = await _moviesImageService.GetImagesByMovieIdAsync(movieId);
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieImagesByMovieTitle(string title)
        {
            var images = await _moviesImageService.GetMovieImagesByMovieTitleAsync(title);
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }


    }
}
