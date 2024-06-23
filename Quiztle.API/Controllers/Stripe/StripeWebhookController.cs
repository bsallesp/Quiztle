using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripeWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                Console.WriteLine("Getting stripe webhook...");

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    "whsec_c2a8c24e141c856b14b24f087fe384bcd18fb0101f257d7bb54477d59b5cac3e"
                );

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine($"{paymentIntent.Amount}");
                    Console.WriteLine("var paymentIntent = stripeEvent.Data.Object as PaymentIntent;");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"error: {e.Message}");
                return BadRequest();
            }

        }
    }
}
