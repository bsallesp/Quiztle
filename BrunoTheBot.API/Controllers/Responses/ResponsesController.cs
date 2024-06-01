using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BrunoTheBot.API.Controllers.Responses
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly ResponseRepository _responseRepository;

        public ResponsesController(ResponseRepository responseRepository)
        {
            _responseRepository = responseRepository ?? throw new ArgumentNullException(nameof(responseRepository));
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<List<Response>>>> GetAllResponses()
        {
            try
            {
                var responses = await _responseRepository.GetAllAsync();
                if (responses == null || !responses.Any())
                {
                    return Ok(new APIResponse<List<Response>>
                    {
                        Status = CustomStatusCodes.EmptyObjectErrorStatus,
                        Data = new List<Response>(),
                        Message = "No responses found."
                    });
                }

                return Ok(new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = responses.ToList(),
                    Message = "Data retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Response>(),
                    Message = $"An error occurred while retrieving data: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetResponseById(Guid id)
        {
            var response = await _responseRepository.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateResponse(Response response)
        {
            try
            {
                await _responseRepository.AddAsync(response);
                return CreatedAtAction(nameof(GetResponseById), new { id = response.Id }, response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResponse(Guid id, Response response)
        {
            if (id != response.Id)
            {
                return BadRequest();
            }

            try
            {
                await _responseRepository.UpdateAsync(response);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(Guid id)
        {
            var response = await _responseRepository.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            await _responseRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("existsByTestId/{testId}")]
        public async Task<ActionResult<APIResponse<bool>>> ExistsByTestId(Guid testId)
        {
            try
            {
                var exists = await _responseRepository.ExistsByTestIdAsync(testId);
                return Ok(new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = exists,
                    Message = exists ? "Response found for the given Test ID." : "No response found for the given Test ID."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = $"An error occurred while checking existence: {ex.Message}"
                });
            }
        }
    }
}
