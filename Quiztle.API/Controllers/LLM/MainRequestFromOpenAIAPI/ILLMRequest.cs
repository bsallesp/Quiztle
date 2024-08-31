namespace Quiztle.API
{
    public interface ILLMRequest
    {
        Task<string> ExecuteAsync(string input, string systemProfile = "");
    }
}