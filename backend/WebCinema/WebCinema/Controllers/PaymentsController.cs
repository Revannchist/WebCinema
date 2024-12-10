using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;
        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(Payments payment)
        {
            var createdPayment = await _paymentsService.CreatePaymentsAsync(payment);
            if (createdPayment == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdPayment);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePaymentById(int id)
        {
            var deletedPayment = await _paymentsService.DeletePaymentsByIdAsync(id);
            if (deletedPayment == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedPayment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePayment(int id, Payments payment)
        {
            var updatedPayment = await _paymentsService.UpdatePaymentsAsync(id, payment);
            if (updatedPayment == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedPayment);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentsService.GetPaymentsByIdAsync(id);
            if (payment == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(payment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentsService.GetAllPaymentsAsync();
            if (payments == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(payments);
        }

    }
}
