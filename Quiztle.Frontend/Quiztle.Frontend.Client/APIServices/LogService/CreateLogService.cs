using System.Net.Http.Json;
using Quiztle.CoreBusiness.Log;

namespace Quiztle.Blazor.Client.APIServices
{
    public class CreateLogService
    {
        private readonly HttpClient _httpClient;
    
        public CreateLogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ExecuteAsync(Log log)
        {
            try
            {
                var url = "api/CreateLog/";
                var httpResponse = await _httpClient.PostAsJsonAsync(url, log);

                Console.WriteLine(httpResponse.Headers.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}