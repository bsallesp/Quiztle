namespace Quiztle.API
{
    public interface IChatGPTRequest
    {
        Task<string> ExecuteAsync(string input, string systemProfile = "");
    }
}