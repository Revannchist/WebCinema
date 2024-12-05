using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesService _countryService;
        public CountriesController(ICountriesService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(Countries country)
        {
            var createdCountry = await _countryService.CreateCountryAsync(country);
            if(createdCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdCountry);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCountryById(int id)
        {
            var deletedCountry = await _countryService.DeleteCountryByIdAsync(id);
            if(deletedCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedCountry);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCountry(int id, Countries country) 
        {
            var updatedCountry = await _countryService.UpdateCountryAsync(id, country);
            if (updatedCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedCountry);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            if(country == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(country);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries() 
        {
            var countries = await _countryService.GetAllCountriesAsync();
            if(countries == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(countries);
        }
    }
}