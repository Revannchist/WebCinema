using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
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
        public async Task<IActionResult> AddRatings(RatingCreateDto ratingDto)
        {
            try
            {
                var createdRatings = await _ratingsService.CreateRatingAsync(ratingDto);
                return Ok(createdRatings);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // This will return "This user has already rated the movie."
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while creating the rating.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRatingsById(int id)
        {
            var deletedRatings = await _ratingsService.DeleteRatingByIdAsync(id);
            if (deletedRatings == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedRatings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRatings(int id, RatingUpdateDto ratingDto)
        {
            var updatedRatings = await _ratingsService.UpdateRatingsAsync(id, ratingDto);
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
