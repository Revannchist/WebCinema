using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimesService _showtimesService;
        private readonly ILogger<ShowTimesController> _logger;

        public ShowTimesController(IShowTimesService showtimesService, ILogger<ShowTimesController> logger)
        {
            _showtimesService = showtimesService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddShowTime(ShowTimes showTimes, CancellationToken cancellationToken)
        {
            try
            {
                var createdShowTimes = await _showtimesService.CreateShowTimesAsync(showTimes, cancellationToken);
                if (createdShowTimes == null)
                {
                    return BadRequest("Error creating show time!");
                }
                return Ok(createdShowTimes);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Add show time operation was canceled");
                return StatusCode(499, "Request canceled");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteShowTimeById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deletedShowtimes = await _showtimesService.DeleteShowTimesByIdAsync(id, cancellationToken);
                if (deletedShowtimes == null)
                {
                    return NotFound($"Show time with ID {id} not found");
                }
                return Ok(deletedShowtimes);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Delete show time operation was canceled for ID: {Id}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateShowTime(int id, ShowTimesUpdateDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                var updatedShowTimes = await _showtimesService.UpdateShowTimesAsync(id, updateDto, cancellationToken);
                if (updatedShowTimes == null)
                {
                    return NotFound($"Show time with ID {id} not found");
                }
                return Ok(updatedShowTimes);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Update show time operation was canceled for ID: {Id}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetShowTimeById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var showtimes = await _showtimesService.GetShowTimesByIdAsync(id, cancellationToken);
                if (showtimes == null)
                {
                    return NotFound($"Show time with ID {id} not found");
                }
                return Ok(showtimes);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get show time by ID operation was canceled for ID: {Id}", id);
                return StatusCode(499, "Request canceled");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllShowTimes(CancellationToken cancellationToken)
        {
            try
            {
                var showtimes = await _showtimesService.GetAllShowTimesAsync(cancellationToken);
                if (showtimes == null || !showtimes.Any())
                {
                    return NoContent();
                }
                return Ok(showtimes);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get all show times operation was canceled");
                return StatusCode(499, "Request canceled");
            }
        }
    }
}