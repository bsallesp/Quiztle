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
                        var paidEntity = new Paid
                        {
                            UserEmail = sessionCompleted.CustomerEmail ?? "",
                            PriceId = sessionCompleted.Metadata["price_id"],
                        };

                        Console.WriteLine("creating _paidRepository.CreatePaidAsync....");
                        await _paidRepository.CreatePaidAsync(paidEntity);

                        Console.WriteLine(sessionCompleted.AmountTotal);
                        //Console.WriteLine(sessionCompleted.Customer.Email);

                        Console.WriteLine(sessionCompleted.Metadata.Count);
                        foreach (var item in sessionCompleted.Metadata)
                        {
                            Console.WriteLine("---------------------------");
                            Console.WriteLine(item.Key);
                            Console.WriteLine(item.Value);
                            Console.WriteLine("---------------------------");
                        }
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
