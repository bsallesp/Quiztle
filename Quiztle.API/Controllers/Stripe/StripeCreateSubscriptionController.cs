using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripeCreateSubscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeCreateSubscriptionController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
        }

        [HttpPost("create-subscription")]
        public IActionResult ExecuteAsync(string customerId, string priceId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = priceId,
                    },
                },
                Expand = new List<string> { "latest_invoice.payment_intent" },
            };

            var service = new SubscriptionService();
            Subscription subscription = service.Create(options);

            return Ok(subscription);
        }
    }
}
