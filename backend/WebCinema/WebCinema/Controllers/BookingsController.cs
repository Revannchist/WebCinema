using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;
        private readonly ILogger<BookingsService> _logger;

        public BookingsController(ILogger<BookingsService> logger, IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingsAddDto bookingDto, CancellationToken cancellationToken)
        {
            try
            {
                var createdBooking = await _bookingsService.CreateBookingsAsync(bookingDto, cancellationToken);
                if (createdBooking == null)
                {
                    return BadRequest("Error creating booking!");
                }
                return Ok(createdBooking);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Create booking operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating booking");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteBookingsById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deletedBookings = await _bookingsService.DeleteBookingsByIdAsync(id, cancellationToken);
                if (!deletedBookings)
                {
                    return BadRequest("Error deleting booking!");
                }
                return Ok(new { success = true, message = "Booking deleted successfully" });
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Delete booking operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking");
                return StatusCode(500, "An unexpected error occurred");
            }
        }


        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "User")]

        [HttpPost]
        public async Task<IActionResult> UpdateBookings(int id, BookingsEditDto bookingsDto, CancellationToken cancellationToken)
        {
            try
            {
                var updatedBookings = await _bookingsService.UpdateBookingsAsync(id, bookingsDto, cancellationToken);
                if (updatedBookings == null)
                {
                    return BadRequest("Error updating booking!");
                }
                return Ok(updatedBookings);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Update booking operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking");
                return StatusCode(500, "An unexpected error occurred");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetBookingsById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var bookings = await _bookingsService.GetBookingsByIdAsync(id, cancellationToken);
                if (bookings == null)
                {
                    return NotFound("Booking not found");
                }
                return Ok(bookings);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get booking operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving booking");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings(CancellationToken cancellationToken)
        {
            try
            {
                var bookings = await _bookingsService.GetAllBookingsAsync(cancellationToken);
                if (bookings == null || !bookings.Any())
                {
                    return NotFound("No bookings found");
                }
                return Ok(bookings);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Get all bookings operation was canceled");
                return StatusCode(499, "Request canceled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving bookings");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
