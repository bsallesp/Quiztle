using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Quiztle.API.Controllers.StripeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeSessionsController : ControllerBase
    {
        [HttpGet("sessions/all")]
        public ActionResult GetAllSessions()
        {
            var options = new Stripe.Checkout.SessionListOptions { Limit = 3 };
            var service = new Stripe.Checkout.SessionService();
            StripeList<Stripe.Checkout.Session> sessions = service.List(options);

            return Ok(sessions);
        }

        [HttpGet("sessions/createsession")]
        public async Task<ActionResult> CreateSession(string priceID)
        {
            string domain = "http://localhost:5514/";
            string sucessPage = "sucess";
            string cancelPage = "abandoned";

            var options = new SessionCreateOptions
            {
                LineItems =
                [
                    new()
                    {
                        Price = priceID,
                        Quantity = 1,
                    },
                ],
                Mode = "payment",
                SuccessUrl = domain + sucessPage,
                CancelUrl = domain + cancelPage
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(session.Url);
        }
    }
}
