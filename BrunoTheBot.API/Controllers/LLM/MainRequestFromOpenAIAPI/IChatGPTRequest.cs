namespace BrunoTheBot.API
{
    public interface IChatGPTRequest
    {
        Task<string> ExecuteAsync(string input, string systemProfile = "");
    }
}