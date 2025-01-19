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
            try
            {
                var createdDirector = await _directorsService.CreateDirectorAsync(directorDto);
                if (createdDirector == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(createdDirector);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Error | Bad Request!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error | {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateDirector(int id, DirectorUpdateDto directorDto)
        {
            try
            {
                var updatedDirector = await _directorsService.UpdateDirectorAsync(id, directorDto);
                if (updatedDirector == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(updatedDirector);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Error | Bad Request!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Error | Director with ID {id} not found!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error | {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }

    [HttpGet]
    public async Task<IActionResult> GetDirectorById(int id)
    {
        try
        {
            var director = await _directorsService.GetDirectorByIdAsync(id);
            if (director == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(director);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Error | Director with ID {id} not found!");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error | Internal Server Error!");
        }
    }

        [HttpGet]
        public async Task<IActionResult> GetAllDirectors()
        {
            try
            {
                var directors = await _directorsService.GetAllDirectorsAsync();
                if (directors == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(directors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirectorById(int id)
        {
            try
            {
                var deletedDirector = await _directorsService.DeleteDirectorByIdAsync(id);
                if (deletedDirector == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(deletedDirector);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Error | Director with ID {id} not found!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }
    }
}
