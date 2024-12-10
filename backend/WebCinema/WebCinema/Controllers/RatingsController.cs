using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService _ratingsService;
        public RatingsController(IRatingsService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRatings(Ratings ratings)
        {
            var createdRatings = await _ratingsService.CreateRatingsAsync(ratings);
            if (createdRatings == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdRatings);

           
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRatingsById(int id)
        {
            var deletedRatings = await _ratingsService.DeleteRatingsByIdAsync(id);
            if (deletedRatings == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedRatings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRatings(int id, Ratings ratings)
        {
            var updatedRatings = await _ratingsService.UpdateRatingsAsync(id, ratings);
            if (updatedRatings == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedRatings);
        }

        [HttpGet]
        public async Task<IActionResult> GetRatingsById(int id)
        {
            var ratings = await _ratingsService.GetRatingsByIdAsync(id);
            if (ratings == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(ratings);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingsService.GetAllRatingsAsync();
            if (ratings == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(ratings);
        }
    }
}
