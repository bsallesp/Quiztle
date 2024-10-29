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
        public ActionResult ListAllPaymentIntents(int amount = 1)
        {
            var options = new PaymentIntentListOptions { Limit = amount };
            var service = new PaymentIntentService();
            StripeList<PaymentIntent> paymentIntents = service.List(options);

            return Ok(paymentIntents);
        }

        [HttpGet("listpaymentintentbyid/{paymentIntentId}")]
        public ActionResult<PaymentIntentDto> ListPaymentIntentById(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            try
            {
                // Obtenha o PaymentIntent usando o ID fornecido
                PaymentIntent paymentIntent = service.Get(paymentIntentId);

                // Mapeie o PaymentIntent para o PaymentIntentDto
                var paymentIntentDto = new PaymentIntentDto
                {
                    Id = paymentIntent.Id,
                    Amount = paymentIntent.Amount,
                    AmountCapturable = paymentIntent.AmountCapturable,
                    AmountReceived = paymentIntent.AmountReceived,
                    Currency = paymentIntent.Currency,
                    Status = paymentIntent.Status,
                    Created = paymentIntent.Created,
                    ClientSecret = paymentIntent.ClientSecret,
                    LastPaymentError = paymentIntent.LastPaymentError?.Message,
                    PaymentMethodTypes = paymentIntent.PaymentMethodTypes,
                    Livemode = paymentIntent.Livemode,
                    Metadata = paymentIntent.Metadata,
                    ReceiptEmail = paymentIntent.ReceiptEmail,
                    Description = paymentIntent.Description,
                    NextAction = paymentIntent.NextAction,
                };

                return Ok(paymentIntentDto);
            }
            catch (StripeException ex)
            {
                return NotFound(new { error = ex.Message });
            }
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