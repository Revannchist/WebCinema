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
        public IActionResult AddCountry(Countries country)
        {
            var createdCountry = _countryService.CreateCountry(country);
            if(createdCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdCountry);
        }


        [HttpPost]
        public IActionResult DeleteCountryById(int id)
        {
            var deletedCountry = _countryService.DeleteCountryById(id);
            if(deletedCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedCountry);
        }

        [HttpPost]
        public IActionResult UpdateCountry(int id, Countries country) 
        {
            var updatedCountry = _countryService.UpdateCountry(id, country);
            if (updatedCountry == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedCountry);
        }

        [HttpGet]
        public IActionResult GetCountryById(int id)
        {
            var country = _countryService.GetCountryById(id);
            if(country == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(country);
        }

        [HttpGet]
        public IActionResult GetAllCountries() 
        {
            var countries = _countryService.GetAllCountries();
            if(countries == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(countries);

        }

    }
}