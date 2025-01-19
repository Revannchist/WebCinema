using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ActorsController : ControllerBase
    {
        private readonly IActorsService _actorsService;
        public ActorsController(IActorsService actorsService)
        {
            _actorsService = actorsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddActor(ActorCreateDto actorDto)
        {
            try
            {
                var createdActor = await _actorsService.CreateActorAsync(actorDto);
                if (createdActor == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(createdActor);
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
        public async Task<IActionResult> DeleteActorById(int id)
        {
            try
            {
                var deletedActor = await _actorsService.DeleteActorByIdAsync(id);
                if (deletedActor == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(deletedActor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Error | Actor with ID {id} not found!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActor(int id, ActorUpdateDto actorDto)
        {
            try
            {
                var updatedActor = await _actorsService.UpdateActorsAsync(id, actorDto);
                if (updatedActor == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(updatedActor);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Error | Bad Request!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Error | Actor with ID {id} not found!");
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
        public async Task<IActionResult> GetActorById(int id)
        {
            try
            {
                var actor = await _actorsService.GetActorByIdAsync(id);
                if (actor == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(actor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Error | Actor with ID {id} not found!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActors()
        {
            try
            {
                var actors = await _actorsService.GetAllActorsAsync();
                if (actors == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(actors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error | Internal Server Error!");
            }
        }
    }
}
