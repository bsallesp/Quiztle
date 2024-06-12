using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;

namespace Quiztle.API.Controllers.Responses
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

        [HttpGet("notfinalized/{testId}")]
        public async Task<ActionResult<APIResponse<Response>>> GetUnfinalizedResponseByTestId(Guid testId)
        {
            var response = await _responseRepository.GetResponseNotFinalized(testId);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<Response>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new Response(),
                    Message = "Response not found in repository - " + response.Message
                });

            return Ok(new APIResponse<Response>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Response found in repository - " + response.Message
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<Response>>> GetResponseById(Guid id)
        {
            var response = await _responseRepository.GetResponseById(id);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<Response>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new Response(),
                    Message = "Response not found in repository - " + response.Message
                });

            return Ok(new APIResponse<Response>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Response found in repository - " + response.Message
            });
        }

        [HttpGet("finalized/{testId}")]
        public async Task<ActionResult<APIResponse<List<Response>>>> GetFinalizedResponsesByTestId(Guid testId)
        {
            var response = await _responseRepository.GetFinalizedResponsesByTestId(testId);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Response>(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<List<Response>>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new List<Response>(),
                    Message = "Finalized responses not found in repository for test ID - " + response.Message
                });

            return Ok(new APIResponse<List<Response>>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Finalized responses found in repository for test ID - " + response.Message
            });
        }


        [HttpPost]
        public async Task<ActionResult<APIResponse<Response>>> CreateResponse(Response response)
        {
            try
            {
                var result = await _responseRepository.CreateResponse(response);
                if (!result)
                    return BadRequest(new APIResponse<Response>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new Response(),
                        Message = "Failed to create the response."
                    });

                return CreatedAtAction(nameof(GetResponseById), new { id = response.Id }, new APIResponse<Response>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = response,
                    Message = "Response created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "An error occurred while creating the response." + ex.Message
                });
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<Response>>> UpdateResponse(Guid id, [FromBody] Response updatedResponse)
        {
            Console.WriteLine("Getting into UpdateResponse in ResponsesController...");

            var response = await _responseRepository.UpdateResponse(id, updatedResponse);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<Response>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new Response(),
                    Message = "Response not found in repository - " + response.Message
                });

            return Ok(new APIResponse<Response>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Response updated successfully in repository - " + response.Message
            });
        }
    }
}
