using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using static Quiztle.API.Controllers.Stripe.StripeCheckoutController;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripePaymentLinkController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripePaymentLinkController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
        }

        [HttpPost("payment-link")]
        public IActionResult CreatePayment([FromBody] CheckoutSessionRequest request)
        {
            try
            {

                var options = new PaymentLinkCreateOptions
                {
                    LineItems = new List<PaymentLinkLineItemOptions>
                    {
                        new PaymentLinkLineItemOptions
                        {
                            Price = "price_1QAyaQAejqbERDlaAMlPawZ3",
                            Quantity = 1,
                            
                        },
                    },
                };
                var service = new PaymentLinkService();
                var result = service.Create(options);

                return Ok(new { result });
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe error type: {e.StripeError.Type}");
                Console.WriteLine($"Code: {e.StripeError.Code}");
                Console.WriteLine($"Message: {e.StripeError.Message}");

                return BadRequest(new { error = e.StripeError.Message });
            }
        }
    }
}