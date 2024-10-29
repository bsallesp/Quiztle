using Microsoft.AspNetCore.Mvc;
using Stripe;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Quiztle.API.Controllers.StripeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeInvoiceController : ControllerBase
    {
        [HttpGet("invoices/all")]
        public ActionResult GetAllInvoices()
        {
            var options = new InvoiceListOptions {  };
            var service = new InvoiceService();

            StripeList<Invoice> invoices = service.List(options);

            return Ok(invoices);
        }

        [HttpGet("invoices/bycustomerId")]
        public ActionResult GetInvoicesByCustomerId(string customerId = "")
        {
            var options = new InvoiceListOptions { Limit = 3, Customer = customerId };
            var service = new InvoiceService();

            StripeList<Invoice> invoices = service.List(options);

            return Ok(invoices);
        }
    }
}
