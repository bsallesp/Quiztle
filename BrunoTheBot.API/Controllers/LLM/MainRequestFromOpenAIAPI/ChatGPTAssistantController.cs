//using Microsoft.AspNetCore.Mvc;
//using OpenAI_API;
//using OpenAI_API.Chat;
//using OpenAIModels = OpenAI_API.Models.Model;
//using BrunoTheBot.DataContext.Repositories;

//namespace BrunoTheBot.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ChatGPTAssistantController : ControllerBase
//    {
//        public BookRepository BookRepository { get; set; }
//        public readonly OpenAIAPI api = new OpenAIAPI("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");
//        public OpenAIModels defaultModel = OpenAIModels.GPT4_Turbo;

//        public ChatGPTAssistantController()
//        {
//            var chat = api.Chat.CreateConversation();
//            chat.Model = OpenAI_API.Models.Model.GPT4_Turbo;
//        }

//        [HttpPost("GetResponse")]
//        public async Task<string> GetResponse(string input)
//        {
//            ChatRequest chatRequest = new ChatRequest()
//            {
//                Model = defaultModel,
//                Temperature = 0.0,
//                MaxTokens = 4096,
//                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
//                Messages = new ChatMessage[] {
//                new ChatMessage(ChatMessageRole.System, @"

//                Ensure that the ""authors"" property is structured as an array (JSON array) containing objects representing authors. Here's an example of how the corrected JSON structure should look:

//                {
//                  ""name"": ""Book Name"",
//                  ""chapters"": [
//                    {
//                      ""name"": ""class name"",
//                      ""places"": [
//                        {
//                          ""name"": ""Place 1""
//                        }
//                      ],
//                      ""authors"": [
//                        {
//                          ""name"": ""Author 1""
//                        },
//                        {
//                          ""name"": ""Author 2""
//                        }
//                      ]
//                    }
//                  ]
//                }

//                Each ""authors"" property should be enclosed within square brackets to denote an array, and each author object should be separated by commas within that array.

//                The resulting JSON should be able to be serialized using JsonConvert.DeserializeObject.

//                The Book entity will be the only input, and you should conduct thorough and academically accurate research on the content so that we can develop a concise and precise lesson and learning plan.

//                 "),
//                new ChatMessage(ChatMessageRole.User, input)
//            }
//            };

//            var results = await api.Chat.CreateChatCompletionAsync(chatRequest);
//            Console.WriteLine(results);
//            /* prints:
//            {
//                "wins": {
//                2020: "Los Angeles Dodgers"
//                }
//            }
//            */

//            FromJSONToBook getBook = new FromJSONToBook();
//            await BookRepository.CreateBookAsync(getBook.GetBookDetailsFromLLM(results.Choices[0].ToString()));

//            return results.Choices[0].ToString();
//        }
//    }
//}
