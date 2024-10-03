using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Verify.V2;

namespace Quiztle.API.Controllers.Twilio
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioTest : ControllerBase
    {
        [HttpPost("TwilioTest")]
        public void ExecuteAsync()
        {
            string accountSid = "AC8a0d05ed2f1b1d6d103a40efd9dd61c2";
            string authToken = "935bb7d0329773424e382ab436f5891d";

            TwilioClient.Init(accountSid, authToken);

            var service = ServiceResource.Create(friendlyName: "My First Verify Service");

            Console.WriteLine(service.Sid);
        }
    }
}
