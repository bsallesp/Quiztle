using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.FromLLMControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromLLMToLogController(IChatGPTRequest chatGPTAPI, AILogRepository aiLogDb) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly AILogRepository aiLogDb = aiLogDb;

        [HttpPost]
        public async Task SaveLog(string name, string json)
        {
            await aiLogDb.CreateAILogAsync(new AILog
            {
                Name = name,
                JSON = json
            });
        }
    }
}