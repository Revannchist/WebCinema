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
        public BookingsController(IBookingsService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        [HttpPost]
        public async Task<ActionResult<Bookings>> AddBooking(BookingsAddDto bookingDto)
        {
            var booking = new Bookings
            {
                UsersId = bookingDto.UsersId,
                ShowTimesId = bookingDto.ShowTimesId,
                TicketQuantity = bookingDto.TicketQuantity, // Will be overridden by service if Booked_Seats exists
                TotalPrice = bookingDto.TotalPrice, // Will be overridden by service if Booked_Seats exists
                BookingStatus = bookingDto.BookingStatus,
                BookingDate = bookingDto.BookingDate ?? DateTime.UtcNow,
                BookedSeats = bookingDto.BookedSeatsIds.Select(seatId => new BookedSeats
                {
                    SeatsId = seatId
                }).ToList()
            };

            var result = await _bookingsService.CreateBookingsAsync(booking);
            return Ok(new
            {
                success = true,
                message = "Booking added successfully",
                bookingId = result.Id
            });
        }


        //[HttpPost]
        //public async Task<IActionResult> UpdateBooking(int id, BookingsEditDto dto)
        //{
        //    var updatedBooking = await _bookingsService.UpdateBookingsBasicInfoAsync(id, dto);
        //    if (updatedBooking == null)
        //    {
        //        return BadRequest("Error | Booking not found or update failed!");
        //    }
        //    return Ok(updatedBooking);
        //}

        [HttpPost]
        public async Task<IActionResult> DeleteBookingsById(int id)
        {
            var deletedBookings = await _bookingsService.DeleteBookingsByIdAsync(id);
            if (deletedBookings == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedBookings);
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateBookings(int id, Bookings bookings)
        //{
        //    var updatedBookings = await _bookingsService.UpdateBookingsAsync(id, bookings);
        //    if (updatedBookings == null)
        //    {
        //        return BadRequest("Greska!");
        //    }
        //    return Ok(updatedBookings);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetBookingsById(int id)
        //{
        //    var bookings = await _bookingsService.GetBookingsByIdAsync(id);
        //    if (bookings == null)
        //    {
        //        return BadRequest("Error | Bad Request!");
        //    }
        //    return Ok(bookings);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingsService.GetAllBookingsAsync();
            if (bookings == null || !bookings.Any())
            {
                return BadRequest("No bookings");
            }
            return Ok(bookings);
        }
    }
}
