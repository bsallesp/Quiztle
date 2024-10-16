using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripeTestController : Controller
    {
        public StripeTestController()
        {
            StripeConfiguration.ApiKey = "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"; // Replace with your key
        }

        [HttpGet("create")]
        public IActionResult CreateResource(string customerId)
        {
            var options = new CustomerCreateOptions
            {
                Email = customerId
            };

            var service = new CustomerService();
            Customer customer = service.Create(options);

            Console.WriteLine(customer.Email);

            return Ok(customer);
        }

        [HttpGet("check/{customerId}")]
        public IActionResult CheckResource(string customerId)
        {
            var service = new CustomerService();
            Customer customer = service.Get(customerId);

            Console.WriteLine(customer.Email);

            return Ok(customer);
        }
    }
}
