using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimesService _showtimesService;
        public ShowTimesController(IShowTimesService showtimesService)
        {
            _showtimesService = showtimesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddShowTimes(ShowTimes showTimes)
        {
            var createdShowTimes = await _showtimesService.CreateShowTimesAsync(showTimes);
            if (createdShowTimes == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdShowTimes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShowTimesById(int id)
        {
            var deletedShowtimes = await _showtimesService.DeleteShowTimesByIdAsync(id);
            if (deletedShowtimes == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedShowtimes);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShowTimes(int id, ShowTimes showTimes)
        {
            var updatedShowTimes = await _showtimesService.UpdateShowTimesAsync(id, showTimes);
            if (updatedShowTimes == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedShowTimes);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowTimesById(int id)
        {
            var showtimes = await _showtimesService.GetShowTimesByIdAsync(id);
            if (showtimes == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(showtimes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShowTiemes()
        {
            var showtimes = await _showtimesService.GetAllShowTimesAsync();
            if (showtimes == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(showtimes);
        }
    }
}
