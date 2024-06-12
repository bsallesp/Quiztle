using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Options
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly OptionRepository _optionRepository;

        public OptionsController(OptionRepository optionRepository)
        {
            _optionRepository = optionRepository ?? throw new ArgumentNullException(nameof(optionRepository));
        }

        [HttpGet("options/{optionsId}")]
        public async Task<ActionResult<APIResponse<List<Option>>>> GetOptionsById(Guid optionsId)
        {
            var response = await _optionRepository.GetOptionByIdAsync(optionsId);

            if (response.Status == CustomStatusCodes.ErrorStatus)
                return StatusCode(500, new APIResponse<List<Option>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Option>(),
                    Message = "ERROR IN REPOSITORY - " + response.Message
                });

            if (response.Status == CustomStatusCodes.NotFound)
                return NotFound(new APIResponse<List<Option>>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = new List<Option>(),
                    Message = "Shots not found in repository - " + response.Message
                });

            return Ok(new APIResponse<List<Option>>
            {
                Status = CustomStatusCodes.SuccessStatus,
                Data = response.Data,
                Message = "Shots found in repository - " + response.Message
            });
        }
    }
}

