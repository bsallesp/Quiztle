namespace BrunoTheBot.API
{
    public interface IChatGPTRequest
    {
        Task<string> ChatWithGPT(string input, string systemProfile = "");
    }
}