using Quiztle.API.Controllers.LLM.Interfaces;

namespace Quiztle.API.Controllers.LLM
{
    public class NgrokEndpointProvider : IEndpointProvider
    {
        private readonly IConfiguration _configuration;

        public NgrokEndpointProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetNgrokEndpoint()
        {
            var ngrokEndpoint = Environment.GetEnvironmentVariable("NGROK_ENDPOINT");

            if (string.IsNullOrEmpty(ngrokEndpoint))
            {
                Console.WriteLine("NGROK_ENDPOINT DOCKER .ENV VARIABLE DOESN'T EXIST. GETTING DEV VARIABLE IN APPSETTINGS...");
                ngrokEndpoint = _configuration["NGROK_ENDPOINT"];

                if (string.IsNullOrEmpty(ngrokEndpoint))
                {
                    throw new Exception("COULDN'T GET NGROK VALUE ANYWHERE.");
                }
            }

            var finalEndpoint = ngrokEndpoint + "/api/generate";

            Console.WriteLine("Endpoint acquired: " + finalEndpoint);
            return finalEndpoint;
        }
    }
}
