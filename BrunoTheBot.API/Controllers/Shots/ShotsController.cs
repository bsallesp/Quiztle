using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.API.Controllers.Shots
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShotsController : ControllerBase
    {
        private readonly ShotRepository _shotRepository;

        public ShotsController(ShotRepository shotRepository)
        {
            _shotRepository = shotRepository ?? throw new ArgumentNullException(nameof(shotRepository));
        }

        [HttpGet("shots/{responseId}")]
        public async Task<ActionResult<APIResponse<Shot>>> GetShotByResponseId(Guid responseId)
        {
            var response = await _shotRepository.GetShotByResponseId(responseId);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<List<Shot>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Shot>(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status == CustomStatusCodes.NotFound)
                return NotFound(new APIResponse<List<Shot>>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new List<Shot>(),
                    Message = "Shots not found in repository - " + response.Message
                });

            return Ok(new APIResponse<Shot>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Shots found in repository - " + response.Message
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<Shot>>> GetShotById(Guid id)
        {
            var response = await _shotRepository.GetShotByResponseId(id);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Shot(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new Shot(),
                    Message = "Shot not found in repository - " + response.Message
                });

            return Ok(new APIResponse<Shot>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Shot found in repository - " + response.Message
            });
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<Shot>>> CreateShot(Shot shot)
        {
            try
            {
                var response = await _shotRepository.CreateShot(shot);
                if (response.Status != CustomStatusCodes.SuccessStatus)
                    return BadRequest(new APIResponse<Shot>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new Shot(),
                        Message = "Failed to create the shot."
                    });

                return CreatedAtAction(nameof(GetShotById), new { id = shot.Id }, new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = shot,
                    Message = "Shot created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Shot(),
                    Message = "An error occurred while creating the shot." + ex.Message
                });
            }
        }

        [HttpDelete("{shotId}/{responseId}")]
        public async Task<ActionResult<APIResponse<bool>>> DeleteShot(Guid shotId, Guid responseId)
        {
            var response = await _shotRepository.DeleteShot(shotId, responseId);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status != CustomStatusCodes.SuccessStatus)
                return NotFound(new APIResponse<bool>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = false,
                    Message = "Shot not found in repository - " + response.Message
                });

            return Ok(new APIResponse<bool>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = true,
                Message = "Shot deleted successfully in repository - " + response.Message
            });
        }
    }
}
