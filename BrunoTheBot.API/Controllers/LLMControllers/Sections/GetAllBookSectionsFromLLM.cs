using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.LLMControllers
{
    public class GetAllBookSectionsFromLLM(IChatGPTRequest chatGPTAPI, SaveAILogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly SaveAILogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync(Book book, int sectionsAmount = 5)
        {
            try
            {
                var prompt = "";
                var newBook = new Book();
                newBook.Name = book.Name;

                foreach (var chapter in book.Chapters)
                {
                    prompt = LLMPrompts.GetNewSectionsFromChapters(book, sectionsAmount);
                    var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt);
                    await _fromLLMToLogController.ExecuteAsync(nameof(ExecuteAsync), responseLLM);
                    var sections = JSONConverter.ConvertToSections(responseLLM, "NewSections");
                    chapter.Sections.AddRange(sections);
                    newBook.Chapters.Add(chapter);
                }

                APIResponse<Book> bookAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = newBook,
                    Message = ""
                };

                return bookAPIResponse ?? throw new Exception();
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
