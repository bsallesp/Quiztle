using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.LLMControllers
{
    public class GetQuestionsFromLLM(IChatGPTRequest chatGPTAPI, AILogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly AILogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<BookAPIResponse>> GetFullNewQuestionsGroupFromLLM(Book book, int questionsPerSection = 1)
        {
            try
            {
                if (book == null || book.Chapters.Count <= 0) return new BookAPIResponse
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    Book = new()
                };

                foreach (var chapter in book.Chapters)
                {
                    foreach (var section in chapter.Sections)
                    {
                        var prompt = LLMPrompts.GetNewQuestion(section.Content.Text!, questionsPerSection);
                        var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt) ?? throw new Exception();
                        await _fromLLMToLogController.SaveLog(nameof(GetFullNewQuestionsGroupFromLLM), responseLLM);
                        var newQuestion = JSONConverter.ConvertToQuestion(responseLLM);

                        Console.WriteLine(newQuestion.Name);
                        Console.WriteLine(newQuestion.Answer);

                        section.Questions.Add(newQuestion);
                    }
                }

                BookAPIResponse bookAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Book = book
                };

                return bookAPIResponse;

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
