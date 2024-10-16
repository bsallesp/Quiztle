using Azure.Communication.Email;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private string connectionString = "endpoint=https://vitoria-communication-service.unitedstates.communication.azure.com/;accesskey=DthsPbCqGtzSNyZASNdm1x5IItLCiDDNGcYzyp42jlAVn5QuQ2XxJQQJ99AJACULyCpLA0DrAAAAAZCS5Jg7";

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest emailRequest)
        {
            if (string.IsNullOrWhiteSpace(emailRequest.ToEmail))
                return BadRequest("Recipient email address is required.");
            if (string.IsNullOrWhiteSpace(emailRequest.Subject))
                return BadRequest("Email subject is required.");
            if (string.IsNullOrWhiteSpace(emailRequest.PlainText) && string.IsNullOrWhiteSpace(emailRequest.Html))
                return BadRequest("Email body is required.");

            var emailClient = new EmailClient(connectionString);

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@certcool.com",
                content: new EmailContent(emailRequest.Subject)
                {
                    PlainText = emailRequest.PlainText,
                    Html = emailRequest.Html
                },
                recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(emailRequest.ToEmail) }));

            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    emailMessage);

                if (emailSendOperation.HasValue)
                {
                    return Ok("Email sent successfully.");
                }
                else
                {
                    return StatusCode(500, "Email send operation did not return a result.");
                }
            }
            catch (RequestFailedException ex)
            {
                return BadRequest($"Error sending email: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string PlainText { get; set; }
        public string Html { get; set; }
    }
}
