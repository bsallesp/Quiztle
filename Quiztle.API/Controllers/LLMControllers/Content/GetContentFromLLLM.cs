using Quiztle.API.Prompts;
using Quiztle.API.Services;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.LLMControllers
{
    public class GetContentFromLLLM(ILLMRequest chatGPTAPI) : ControllerBase
    {
        private readonly ILLMRequest _chatGPTRequest = chatGPTAPI;
        //private readonly SaveAILogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync(Book book)
        {
            try
            {
                foreach (var chapter in book.Chapters)
                {
                    foreach (var section in chapter.Sections)
                    {
                        var prompt = CreateBookPrompts.GetNewContentFromSection(book.Name, chapter.Name, section.Name);
                        var responseLLM = await _chatGPTRequest.ExecuteAsync(prompt) ?? throw new Exception();
                        var registerName = book.Name + chapter.Name + section.Name;
                        //await _fromLLMToLogController.ExecuteAsync(registerName, responseLLM);
                        var newContent = JSONConverter.ConvertToContent(responseLLM, "NewContent");
                        if (string.IsNullOrEmpty(newContent)) throw new Exception("The FromLLMToContent amount is zero or null");

                        section.Content.Text = newContent;
                        Console.WriteLine(newContent);
                    }
                }

                APIResponse<Book> bookAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = book
                };

                return bookAPIResponse ?? throw new Exception("bookAPIResponse show some error: ");
            }
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }
    }
}
