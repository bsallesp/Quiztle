using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository.Payments;
using Stripe;

namespace workspace.Controllers
{
    [Route("api/[controller]")]
    public class StripeWebHook : Controller
    {
        private readonly PaidRepository _paidRepository;

        public StripeWebHook(PaidRepository paidRepository)
        {
            _paidRepository = paidRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);

                    Console.WriteLine("PAYMENT SUCESSSSS!!!!!!!!");
                }
                else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
//whsec_753fb15af4e9bf2d2f979e1c20f0a6641305d95790a5ef1bd09b8ce99b61a2aa