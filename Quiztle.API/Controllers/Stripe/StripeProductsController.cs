using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Quiztle.API.Controllers.Stripe
{
    [Route("api/webhooks/stripe")]
    public class StripeProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
        }

        [HttpGet("get-prices/")]
        public IActionResult GetPrices()
        {
            var service = new PriceService();
            var options = new PriceListOptions
            {
                // Aqui você pode adicionar opções de paginação se necessário
            };

            var prices = service.List(options); // Lista todos os preços disponíveis

            return Ok(prices); // Retorna a lista de preços
        }
    }
}
