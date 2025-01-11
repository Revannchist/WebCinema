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
            var createdActor = await _actorsService.CreateActorAsync(actorDto);
            if (createdActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdActor);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActorById(int id)
        {
            var deletedActor = await _actorsService.DeleteActorByIdAsync(id);
            if (deletedActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedActor);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActor(int id, ActorUpdateDto actorDto)
        {
            var updatedActor = await _actorsService.UpdateActorsAsync(id, actorDto);
            if (updatedActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedActor);
        }

        [HttpGet]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _actorsService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(actor);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActors()
        {
            var actors = await _actorsService.GetAllActorsAsync();
            if (actors == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(actors);

        }
    }
}
