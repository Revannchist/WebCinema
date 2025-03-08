using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddShowTime(ShowTimes showTimes)
        {
            var createdShowTimes = await _showtimesService.CreateShowTimesAsync(showTimes);
            if (createdShowTimes == null)
            {
                return BadRequest("Error!");
            }
            return Ok(createdShowTimes);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteShowTimeById(int id)
        {
            var deletedShowtimes = await _showtimesService.DeleteShowTimesByIdAsync(id);
            if (deletedShowtimes == null)
            {
                return BadRequest("Error!");
            }
            return Ok(deletedShowtimes);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateShowTime(int id, ShowTimesUpdateDto updateDto)
        {
            var updatedShowTimes = await _showtimesService.UpdateShowTimesAsync(id, updateDto);
            if (updatedShowTimes == null)
            {
                return BadRequest("Error!");
            }
            return Ok(updatedShowTimes);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetShowTimeById(int id)
        {
            var showtimes = await _showtimesService.GetShowTimesByIdAsync(id);
            if (showtimes == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(showtimes);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllShowTimes()
        {
            var showtimes = await _showtimesService.GetAllShowTimesAsync();
            if (showtimes == null || !showtimes.Any())
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(showtimes);
        }
    }
}
