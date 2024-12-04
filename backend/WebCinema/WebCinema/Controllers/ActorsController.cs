using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
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
        public IActionResult AddActor(Actors actor)
        {
            var createdActor = _actorsService.CreateActor(actor);
            if (createdActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdActor);
        }

        [HttpPost]
        public IActionResult DeleteActorById(int id)
        {
            var deletedActor = _actorsService.DeleteActorById(id);
            if (deletedActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedActor);
        }

        [HttpPost]
        public IActionResult UpdateActor(int id, Actors actor)
        {
            var updatedActor = _actorsService.UpdateActor(id, actor);
            if (updatedActor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedActor);
        }

        [HttpGet]
        public IActionResult GetActorById(int id)
        {
            var actor = _actorsService.GetActorById(id);
            if (actor == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(actor);
        }

        [HttpGet]
        public IActionResult GetAllActors()
        {
            var actors = _actorsService.GetAllActors();
            if (actors == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(actors);

        }
    }
}
