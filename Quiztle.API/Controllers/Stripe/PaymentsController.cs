// This example sets up an endpoint using the ASP.NET MVC framework.
// To learn more about ASP.NET MVC, watch this video: https://youtu.be/2-mMOB8MhmE.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace server.Controllers
{
    public class PaymentsController : Controller
    {
        public PaymentsController()
        {
            StripeConfiguration.ApiKey = "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";
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = 2000,
              Currency = "usd",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = "T-shirt",
              },
            },
            Quantity = 1,
          },
        },
                Mode = "payment",
                UiMode = "embedded",
                ReturnUrl = "https://example.com/return?session_id={CHECKOUT_SESSION_ID}",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { clientSecret = session.ClientSecret });
        }
    }
}