using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorsService _directorsService;
        public DirectorsController(IDirectorsService directorsService)
        {
            _directorsService = directorsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(DirectorCreateDto directorDto)
        {
            var createdDirector = await _directorsService.CreateDirectorAsync(directorDto);
            if (createdDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdDirector);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirectorById(int id)
        {
            var deletedDirector = await _directorsService.DeleteDirectorByIdAsync(id);
            if (deletedDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedDirector);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDirector(int id, DirectorUpdateDto directorDto)
        {
            var updatedDirector = await _directorsService.UpdateDirectorAsync(id, directorDto);
            if (updatedDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedDirector);
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            var director = await _directorsService.GetDirectorByIdAsync(id);
            if (director == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(director);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDirectors()
        {
            var directors = await _directorsService.GetAllDirectorsAsync();
            if (directors == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(directors);
        }
    }
}
