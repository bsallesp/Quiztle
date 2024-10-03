namespace Quiztle.API.Controllers.LLM.Interfaces
{
    public interface ILLMRequest
    {
        Task<string> ExecuteAsync(string input, CancellationToken cancellationToken, string systemProfile = "");
    }
}