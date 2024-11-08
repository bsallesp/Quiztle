using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using Quiztle.CoreBusiness.Log;
using Quiztle.DataContext.Repositories;
using Stripe;
using Stripe.Checkout;

namespace Quiztle.API.Controllers.StripeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeSessionsController(
        IConfiguration configuration,
        LogRepository logRepository)
        : ControllerBase
    {
        private IConfiguration _configuration = configuration;
        private readonly LogRepository _logRepository = logRepository;

        [HttpGet("sessions/all")]
        public ActionResult GetAllSessions()
        {
            var options = new Stripe.Checkout.SessionListOptions { Limit = 3 };
            var service = new Stripe.Checkout.SessionService();
            StripeList<Stripe.Checkout.Session> sessions = service.List(options);

            return Ok(sessions);
        }

        [HttpPost("sessions/createsession")]
        public async Task<ActionResult> CreateSession([FromBody] SessionStartDTO sessionStartDto)
        {
            var guidLog = Guid.NewGuid();
            await _logRepository.CreateLogAsync(new Log
            {
                Content = sessionStartDto.ToJson(),
                GuidLog = guidLog,
                Name = $"Session Start Attempt",
            });
            
            var sessionStartErrors = new string[4];
            var errorIndex = 0;

            if (sessionStartDto.Email == null) sessionStartErrors[errorIndex++] = "Email cannot be null.";
            if (sessionStartDto.PriceId == null) sessionStartErrors[errorIndex++] = "PriceId cannot be null.";
            if (sessionStartDto.TestId == null) sessionStartErrors[errorIndex++] = "TestId cannot be null.";
            if (sessionStartDto.Amount == 0) sessionStartErrors[errorIndex++] = "Amount cannot be null.";
            if (errorIndex > 0)
            {
                await _logRepository.CreateLogAsync(new Log
                {
                    Content = sessionStartDto.ToJson() + sessionStartErrors[errorIndex++],
                    GuidLog = guidLog,
                    Name = $"Session Start Error"
                });
                
                return BadRequest(string.Join(", ", sessionStartErrors.Take(errorIndex)));
            }

            var domain = Environment.GetEnvironmentVariable("DOMAIN_FRONTEND") ??
                         _configuration["Domains:Frontend"];

            if (domain == null)
            {
                await _logRepository.CreateLogAsync(new Log
                {
                    Content = sessionStartDto.ToJson() + " Domain configuration missing.",
                    GuidLog = guidLog,
                    Name = $"Session Start Error"
                });

                return BadRequest("API domain is not configured.");
            }
            
            string successPage = $"/games/Arcade/{sessionStartDto.TestId}";
            string cancelPage = $"/games/Arcade/{sessionStartDto.TestId}";

            var options = new SessionCreateOptions
            {
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        Price = sessionStartDto.PriceId,
                        Quantity = 1
                    }
                ],
                Mode = "payment",
                SuccessUrl = domain + successPage,
                CancelUrl = domain + cancelPage,
                Metadata = new Dictionary<string, string>
                {
                    { "price_id", sessionStartDto.PriceId },
                    { "customer_email", sessionStartDto.Email },
                    { "test_id", sessionStartDto.TestId },
                    { "amount", sessionStartDto.Amount.ToString() }
                }
            };

            try
            {
                var service = new SessionService();
                var session = await service.CreateAsync(options);

                await _logRepository.CreateLogAsync(new Log
                {
                    Id = Guid.NewGuid(),
                    GuidLog = guidLog,
                    Content = sessionStartDto.ToJson() + session.ToJson(),
                    Name = "Session Start - Success"
                });

                return Ok(session.Url);
            }
            catch (StripeException ex)
            {
                await _logRepository.CreateLogAsync(new Log
                {
                    Content = sessionStartDto.ToJson() + "\n" + ex.Message + "\n" + ex.StackTrace,
                    GuidLog = guidLog,
                    Name = $"Create Session /  StripeException:"
                });

                return BadRequest($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                await _logRepository.CreateLogAsync(new Log
                {
                    Content = sessionStartDto.ToJson() + "\n" + ex.Message + "\n" + ex.StackTrace,
                    GuidLog = guidLog,
                    Name = $"Create Session / Exception:"
                });

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("sessions/getpaidsessions")]
        public ActionResult GetPaidSessions()
        {
            var options = new Stripe.Checkout.SessionListOptions
            {
                Limit = 100,
                Status = "complete",
            };
            var service = new SessionService();
            StripeList<Stripe.Checkout.Session> sessions = service.List(options);

            var paidSessions = sessions.Where(session => session.PaymentStatus == "paid").ToList();

            return Ok(paidSessions);
        }

        [HttpGet("sessions/getpaidsessionsbyuserid")]
        public async Task<List<PaidSessionDto>> GetPaidSessionsByUserid(string userId)
        {
            var options = new Stripe.Checkout.SessionListOptions
            {
                Limit = 100,
                Status = "complete",
            };
            var service = new SessionService();
            StripeList<Stripe.Checkout.Session> sessions = await service.ListAsync(options);

            var paidSessions = sessions
                .Where(session => session.PaymentStatus == "paid" && session.ClientReferenceId == userId)
                .ToList();

            var paidSessionDtos = paidSessions.Select(session => new PaidSessionDto
            {
                Id = session.Id,
                PaymentStatus = session.PaymentStatus,
                CustomerEmail = session.CustomerEmail,
                AmountTotal = session.AmountTotal ?? 0,
                Currency = session.Currency,
                SuccessUrl = session.SuccessUrl,
                CancelUrl = session.CancelUrl,
                Created = session.Created,
                PaymentIntentId = session.PaymentIntentId,
            }).ToList();


            return paidSessionDtos;
        }

        [HttpGet("sessions/{sessionId}/lineitems")]
        public ActionResult<PaidSessionDto> GetLineItemsBySessionId(string sessionId)
        {
            var sessionService = new SessionService();
            var lineItemService = new SessionLineItemService();

            try
            {
                // Obtenha a sessão usando o ID fornecido
                var session = sessionService.Get(sessionId);

                // Obtenha os line items associados a essa sessão
                var lineItems = lineItemService.List(sessionId);

                // Crie o DTO com informações relevantes da sessão
                var paidSessionDto = new PaidSessionDto
                {
                    Id = session.Id,
                    PaymentStatus = session.PaymentStatus,
                    CustomerEmail = session.CustomerEmail,
                    AmountTotal = session.AmountTotal ?? 0, // Garantir que não seja nulo
                    Currency = session.Currency,
                    SuccessUrl = session.SuccessUrl,
                    CancelUrl = session.CancelUrl,
                    Created = session.Created,
                    PaymentIntentId = session.PaymentIntentId,
                    LineItems = lineItems.Select(item => new LineItemDto
                    {
                        PriceId = item.Price.Id,
                        Quantity = item.Quantity ?? 0
                    }).ToList()
                };

                return Ok(paidSessionDto);
            }
            catch (StripeException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("sessions/ispayedsession")]
        public async Task<ActionResult<bool>> IsPaidSession(string sessionId, string customerId, string priceId)
        {
            var sessionService = new SessionService();

            try
            {
                // Obtenha a sessão usando o ID fornecido
                var session = await sessionService.GetAsync(sessionId);

                // Verifique se a sessão está paga e se corresponde ao customerId e priceId
                bool isPaid = session.PaymentStatus == "paid" &&
                              session.ClientReferenceId == customerId &&
                              session.LineItems.Any(item => item.Price.Id == priceId);

                return Ok(isPaid);
            }
            catch (StripeException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("sessions/ispayedsessionbycustomer")]
        public async Task<ActionResult<bool>> IsPaidSessionByCustomer(string customerId, string priceId)
        {
            var sessionService = new SessionService();
            var options = new SessionListOptions
            {
                Limit = 100,
                Status = "complete" // Apenas sessões completas
            };

            try
            {
                // Busque todas as sessões
                StripeList<Session> sessions = await sessionService.ListAsync(options);

                // Iterar sobre as sessões para encontrar uma paga que corresponda ao customerId e priceId
                foreach (var session in sessions)
                {
                    if (session.PaymentStatus == "paid" && session.ClientReferenceId == customerId)
                    {
                        // Obtenha os line items associados a esta sessão
                        var lineItemService = new SessionLineItemService();
                        var lineItems = lineItemService.List(session.Id);

                        // Verifique se algum line item tem o priceId desejado
                        if (lineItems.Any(item => item.Price.Id == priceId))
                        {
                            return Ok(true); // Encontra uma sessão paga correspondente
                        }
                    }
                }

                return Ok(false); // Nenhuma sessão paga encontrada
            }
            catch (StripeException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}