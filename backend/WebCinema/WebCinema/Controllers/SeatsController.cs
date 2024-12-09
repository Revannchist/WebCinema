using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatsService _seatsService;
        public SeatsController(ISeatsService seatsService)
        {
            _seatsService = seatsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSeats(Seats seats)
        {
            var createdSeats = await _seatsService.CreateSeatsAsync(seats);
            if (createdSeats == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdSeats);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSeatsById(int id)
        {
            var deletedSeats = await _seatsService.DeleteSeatsByIdAsync(id);
            if (deletedSeats == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedSeats);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeats(int id, Seats seats)
        {
            var updatedSeats = await _seatsService.UpdateSeatsAsync(id, seats);
            if (updatedSeats == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedSeats);
        }

        [HttpGet]
        public async Task<IActionResult> GetSeatsById(int id)
        {
            var seats = await _seatsService.GetSeatsByIdAsync(id);
            if (seats == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(seats);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var seats = await _seatsService.GetAllSeatsAsync();
            if (seats == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(seats);
        }
    }
}
