using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztle.API.Controllers.Stripe
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeCustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeCustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
        }

        [HttpGet("check-customer-existence")]
        public async Task<ActionResult<Customer>> CheckCustomerExistence(string customerEmail)
        {
            try
            {
                var service = new CustomerService();
                var options = new CustomerListOptions
                {
                    Email = customerEmail,
                    Limit = 1
                };

                var customers = await service.ListAsync(options);

                var existingCustomer = customers.Data.FirstOrDefault();

                if (existingCustomer != null)
                {
                    return Ok(new Customer { Email = existingCustomer.Email });
                }
                else
                {
                    return NotFound($"Customer with email {customerEmail} not found.");
                }
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("check-or-create-customer")]
        public async Task<ActionResult<Customer>> CheckOrCreateCustomer(string customerEmail, string customerName)
        {
            try
            {
                var service = new CustomerService();
                var options = new CustomerListOptions
                {
                    Email = customerEmail,
                    Limit = 1
                };

                var customers = await service.ListAsync(options);
                var existingCustomer = customers.Data.FirstOrDefault();

                if (existingCustomer != null)
                {
                    return Ok(new Customer { Email = existingCustomer.Email });
                }
                else
                {
                    // Cria um novo cliente, caso não exista
                    var createOptions = new CustomerCreateOptions
                    {
                        Email = customerEmail,
                        Name = customerName
                    };

                    var newCustomer = await service.CreateAsync(createOptions);

                    return Ok(new Customer { Email = newCustomer.Email });
                }
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }
    }

    public class Customer
    {
        public string? Email { get; set; } = "";
    }
}
