using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Quiztle.API.Controllers.StripeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeCustomerController : ControllerBase
    {
        [HttpGet("search")]
        public ActionResult<object> SearchCustomer(string name, string email)
        {
            return Search(name, email);
        }


        [HttpGet("search/name")]
        public ActionResult<object> SearchCustomerByName(string name)
        {
            var options = new CustomerSearchOptions
            {
                Query = $"name:'{name.ToLower()}'",
            };
            var service = new CustomerService();
            var result = service.Search(options);

            if (result.IsNullOrEmpty()) return NotFound(result);

            return Ok(result);
        }


        [HttpGet("search/email")]
        public ActionResult<string> SearchCustomerByEmail(string email)
        {
            var options = new CustomerSearchOptions
            {
                Query = $"email:'{email.ToLower()}'",
            };
            var service = new CustomerService();
            var result = service.Search(options);

            if (result.FirstOrDefault() == null) return NotFound();

            return Ok(result.FirstOrDefault()!.Email);
        }

        private ActionResult<object> Search(string name = "", string email = "")
        {
            var queryString = $"name:'{name}' OR email:'{email.ToLower()}'";

           
            var options = new CustomerSearchOptions { Query = queryString };

            var service = new CustomerService();
            StripeSearchResult<Customer> customers = service.Search(options);

            if (customers.Data.Count == 0)
            {
                return NotFound(new { message = "No customer found." });
            }

            // Return a simplified JSON response
            var result = customers.Data.Select(c => new
            {
                c.Id,
                c.Name,
                c.Email,
                c.Created,
                c.Description
            });

            return Ok(result);
        }


        [HttpGet("listall")]
        public ActionResult<object> ListAllCustomers()
        {
            var options = new CustomerListOptions { Limit = 3 };
            var service = new CustomerService();
            StripeList<Customer> customers = service.List(options);

            var response = customers.Data.Select(c => new
            {
                c.Id,
                c.Name,
                c.Email,
                c.Created,
                c.Description
            }).ToList();

            Console.WriteLine(response);

            return Ok(response);
        }


        [HttpPost("create")]
        public ActionResult<object> CreateCustomer(string name, string email)
        {
            var options = new CustomerCreateOptions
            {
                Name = name,
                Email = email.ToLower(),
            };
            var service = new CustomerService();
            var result = service.Create(options);

            return CreatedAtAction(nameof(CreateCustomer), result);
        }
    }
}
