namespace Quiztle.API.Controllers.LLM.Interfaces
{
    public interface ILLMChatGPTRequest
    {
        Task<string> ExecuteAsync(string input, string systemProfile = "");
    }
}