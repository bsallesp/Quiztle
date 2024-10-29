using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Paid;
using Quiztle.DataContext.DataService.Repository.Payments;

namespace Quiztle.API.Controllers.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaidController : ControllerBase
    {
        private readonly PaidRepository _paidRepository;

        public PaidController(PaidRepository paidRepository)
        {
            _paidRepository = paidRepository;
        }

        [HttpPost("addpaid")]
        public async Task<IActionResult> AddPaidAsync([FromBody] Paid paid)
        {
            try
            {
                Paid newPaid = new()
                {
                    Id = Guid.NewGuid(),
                    PriceId = paid.PriceId,
                    CustomerId = paid.CustomerId,
                    UserId = paid.UserId,
                    Created = DateTime.UtcNow
                };

                await _paidRepository.CreatePaidAsync(newPaid);

                return Ok(newPaid);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
