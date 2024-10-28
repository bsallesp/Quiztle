using Microsoft.AspNetCore.Mvc;
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
            return Search(name, "");
        }

        [HttpGet("search/email")]
        public ActionResult<object> SearchCustomerByEmail(string email)
        {
            return Search("", email);
        }

        private ActionResult<object> Search(string name = "", string email = "")
        {
            var queryString = $"name:\"{name}\" OR email:\"{email}\"";


            if (string.IsNullOrEmpty(email)) queryString = $"name:\"{name}\"";
            if (string.IsNullOrEmpty(name)) queryString = $"email:\"{email}\"";

            Console.WriteLine(queryString);
            
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
            var searchResult = SearchCustomer(name, email);
            if (searchResult.Result is not null && searchResult.Value is IEnumerable<dynamic> existingCustomers && existingCustomers.Any())
            {
                return Conflict(new { message = "Customer already exists." });
            }

            var options = new CustomerCreateOptions
            {
                Name = name,
                Email = email,
            };

            var service = new CustomerService();
            var newCustomer = service.Create(options);

            // Return a simplified JSON response for the created customer
            var response = new
            {
                newCustomer.Id,
                newCustomer.Name,
                newCustomer.Email,
                newCustomer.Created,
                newCustomer.Description
            };

            return CreatedAtAction(nameof(CreateCustomer), response); // Return 201 Created
        }


    }
}
