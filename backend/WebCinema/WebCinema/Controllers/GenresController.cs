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
        public IActionResult GetGenresById(int id)
        {
            var genre = _genresService.GetGenresById(id);
            if (genre == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(genre);
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            var genres = _genresService.GetAllGenres();
            if (genres == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(genres);

        }
    }
}
