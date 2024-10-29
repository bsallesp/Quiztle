using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe;

namespace Quiztle.API.Controllers.StripeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripePaymentIntentController : ControllerBase
    {
        [HttpGet("listall")]
        public ActionResult ListAllPaymentIntents()
        {
            var options = new PaymentIntentListOptions { Limit = 30 };
            var service = new PaymentIntentService();
            StripeList<PaymentIntent> paymentIntents = service.List(options);

            return Ok(paymentIntents);
        }

        [HttpGet("listbycustomer/{customerId}")]
        public ActionResult ListPaymentIntentsByCustomer(string customerId)
        {
            var options = new PaymentIntentListOptions
            {
                Limit = 30,
                Customer = customerId
            };
            var service = new PaymentIntentService();
            StripeList<PaymentIntent> paymentIntents = service.List(options);

            return Ok(paymentIntents);
        }
    }
}