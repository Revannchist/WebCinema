using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TheatersController : ControllerBase
    {
        private readonly ITheatersService _theatersService;
        public TheatersController(ITheatersService theatersService)
        {
            _theatersService = theatersService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTheaters(Theaters theaters)
        {
            var createdTheaters = await _theatersService.CreateTheatersAsync(theaters);
            if (createdTheaters == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdTheaters);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheatersById(int id)
        {
            var deletedTheaters = await _theatersService.DeleteTheatersByIdAsync(id);
            if (deletedTheaters == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedTheaters);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTheaters(int id, Theaters theaters)
        {
            var updatedTheaters = await _theatersService.UpdateTheatersAsync(id, theaters);
            if (updatedTheaters == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedTheaters);
        }

        [HttpGet]
        public async Task<IActionResult> GetTheatersById(int id)
        {
            var theaters = await _theatersService.GetTheatersByIdAsync(id);
            if (theaters == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(theaters);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheaters()
        {
            var theaters = await _theatersService.GetAllTheatersAsync();
            if (theaters == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(theaters);
        }
    }
}
