using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;
        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesById(int id)
        {
            var cities = await _citiesService.GetCitiesByIdAsync(id);
            if (cities == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(cities);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _citiesService.GetCitiesAsync();
            if (cities == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(cities);
        }
    }
}
