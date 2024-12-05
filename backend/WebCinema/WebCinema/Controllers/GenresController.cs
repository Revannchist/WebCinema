using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;
        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenresById(int id)
        {
            var genre = await _genresService.GetGenresByIdAsync(id);
            if (genre == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(genre);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genresService.GetAllGenresAsync();
            if (genres == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(genres);
        }
    }
}
