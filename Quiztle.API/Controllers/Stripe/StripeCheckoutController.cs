using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Text.Json;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripeCheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeCheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
            
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    ClientReferenceId = request.ClientReferenceId,
                    Customer = request.CustomerId,
                    SuccessUrl = request.SuccessUrl,
                    CancelUrl = request.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = request.PriceId,
                            Quantity = request.Quantity
                        }
                    },
                        Mode ="subscription"
                    };

                var service = new SessionService();
                var session = service.Create(options);

                return Ok(new { sessionId = session.Id });
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe error type: {e.StripeError.Type}");
                Console.WriteLine($"Code: {e.StripeError.Code}");
                Console.WriteLine($"Message: {e.StripeError.Message}");

                return BadRequest(new { error = e.StripeError.Message });
            }
        }
        public class CheckoutSessionRequest
        {
            public string? PriceId { get; set; }
            public int Quantity { get; set; }
            public string? SuccessUrl { get; set; }
            public string? CancelUrl { get; set; }
            public string? CustomerId { get; set; }

            public string? ClientReferenceId { get; set; }
            public string? CustomerEmail { get; set; }
            public bool IsSubscription { get; set; }
        }


        [HttpGet("pendents-checkouts")]
        public IActionResult PendentsCheckouts()
        {
            try
            {
                var service = new SessionService();
                var options = new SessionListOptions
                {
                    Limit = 100, // Limitar o número de sessões retornadas
                };
                var sessions = service.List(options);
                // Filtrar sessões pendentes
                var pendingSessions = sessions.Where(s => s.Status == "open" && s.Mode == "subscription").ToList();
                return Ok(pendingSessions);
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe error type: {e.StripeError.Type}");
                Console.WriteLine($"Code: {e.StripeError.Code}");
                Console.WriteLine($"Message: {e.StripeError.Message}");

                switch (e.StripeError.Type)
                {
                    case "card_error":
                    case "api_connection_error":
                    case "api_error":
                    case "authentication_error":
                    case "invalid_request_error":
                    case "rate_limit_error":
                    case "validation_error":
                        // Handle specific errors if needed
                        break;
                    default:
                        // Unknown Error Type
                        Console.WriteLine("An unknown error occurred.");
                        break;
                }

                return BadRequest(new { error = e.StripeError.Message });
            }
        }
    }
}
