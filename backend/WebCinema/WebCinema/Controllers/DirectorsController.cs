using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;

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
        public IActionResult AddDirector(Directors director)
        {
            var createdDirector = _directorsService.CreateDirector(director);
            if (createdDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdDirector);
        }

        [HttpPost]
        public IActionResult DeleteDirectorById(int id)
        {
            var deletedDirector = _directorsService.DeleteDirectorById(id);
            if (deletedDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedDirector);
        }

        [HttpPost]
        public IActionResult UpdateDirector(int id, Directors director)
        {
            var updatedDirector = _directorsService.UpdateDirector(id, director);
            if (updatedDirector == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedDirector);
        }

        [HttpGet]
        public IActionResult GetDirectorById(int id)
        {
            var director = _directorsService.GetDirectorById(id);
            if (director == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(director);
        }

        [HttpGet]
        public IActionResult GetAllDirectors()
        {
            var directors = _directorsService.GetAllDirectors();
            if (directors == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(directors);

        }
    }
}
