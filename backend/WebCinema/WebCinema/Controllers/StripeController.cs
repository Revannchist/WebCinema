using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StripeController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreatePaymentIntent([FromBody] PaymentIntentCreateRequest request)
        {
            
            StripeConfiguration.ApiKey = "sk_test_51RMaUIR5a4PC69xEKa8nAVHMRNhs200G1hMPJ2egEAFLFM3EtyREKjmRzWfuFdCm4zooTwf3EwJDl8trGEEHXW0i00Tx2bmmfX";

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100), 
                Currency = "eur",
                PaymentMethodTypes = new List<string> { "card" }
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
    }

    
    public class PaymentIntentCreateRequest
    {
        public decimal Amount { get; set; }
    }
}