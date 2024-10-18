//using Microsoft.AspNetCore.Mvc;
//using Stripe;

//namespace Quiztle.API.Controllers.Stripe
//{
//    [Route("api/webhooks/stripe")]
//    public class StripeTestController : Controller
//    {
//        private readonly IConfiguration _configuration;

//        public StripeTestController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//            StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
//        }

//        [HttpGet("create")]
//        public IActionResult CreateResource(string customerId)
//        {
//            var options = new CustomerCreateOptions
//            {
//                Email = customerId
//            };

//            var service = new global::Stripe.CustomerService();
//            Customer customer = service.Create(options);

//            Console.WriteLine(customer.Email);

//            return Ok(customer);
//        }

//        [HttpGet("check/{customerId}")]
//        public IActionResult CheckResource(string customerId)
//        {
//            var service = new global::Stripe.CustomerService();
//            Customer customer = service.Get(customerId);

//            Console.WriteLine(customer.Email);

//            return Ok(customer);
//        }

//        [HttpGet("list")]
//        public IActionResult ListResources()
//        {
//            var service = new global::Stripe.CustomerService();
//            var customers = service.List();

//            string lastId = "";

//            // Enumerate the first page of the list
//            foreach (Customer customer in customers)
//            {
//                lastId = customer.Id;
//                Console.WriteLine(customer.Email);
//            }

//            customers = service.List(new CustomerListOptions()
//            {
//                StartingAfter = lastId,
//            });

//            // Enumerate the subsequent page
//            foreach (Customer customer in customers)
//            {
//                lastId = customer.Id;
//                Console.WriteLine(customer.Email);
//            }

//            return Ok(customers);
//        }

//        [HttpGet("delete")]
//        public IActionResult DeleteResources(string customerId)
//        {
//            var service = new global::Stripe.CustomerService();
//            var customer = service.Delete(customerId);

//            Console.WriteLine(customer.Email);

//            return Ok(customer);
//        }
//    }
//}
