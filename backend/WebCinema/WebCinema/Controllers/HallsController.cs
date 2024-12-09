using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HallsController : ControllerBase
    {
        private readonly IHallsService _hallsService;
        public HallsController(IHallsService hallsServicee)
        {
            _hallsService = hallsServicee;
        }

        [HttpPost]
        public async Task<IActionResult> AddHalls(Halls halls)
        {
            var createdHalls = await _hallsService.CreateHallsAsync(halls);
            if (createdHalls == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdHalls);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHallsById(int id)
        {
            var deletedHalls = await _hallsService.DeleteHallsByIdAsync(id);
            if (deletedHalls == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedHalls);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHalls(int id, Halls halls)
        {
            var updatedHalls = await _hallsService.UpdateHallsAsync(id, halls);
            if (updatedHalls == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedHalls);
        }

        [HttpGet]
        public async Task<IActionResult> GetHallsById(int id)
        {
            var halls = await _hallsService.GetHallsByIdAsync(id);
            if (halls == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(halls);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHalls()
        {
            var halls = await _hallsService.GetAllHallsAsync();
            if (halls == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(halls);
        }
    }
}
