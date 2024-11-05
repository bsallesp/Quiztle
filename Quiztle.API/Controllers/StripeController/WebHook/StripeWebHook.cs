using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository.Payments;
using Stripe;
using Stripe.Checkout;
using Quiztle.CoreBusiness.Entities.Paid;

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

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var sessionCompleted = stripeEvent.Data.Object as Session;

                    if (sessionCompleted != null)
                    {
                        var newPaid = new Paid
                        {
                            UserEmail = sessionCompleted.Metadata["customer_email"] ?? "",
                            PriceId = sessionCompleted.Metadata["price_id"],
                            TestId = sessionCompleted.Metadata["test_id"],
                            Amount = int.Parse(sessionCompleted.Metadata["amount"])
                        };

                        newPaid.Status = "Completed";
                        await _paidRepository.CreatePaidAsync(newPaid);
                    }
                    else
                    {
                        Console.WriteLine("Session object is null.");
                    }
                }
                else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                }
                else
                {
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
