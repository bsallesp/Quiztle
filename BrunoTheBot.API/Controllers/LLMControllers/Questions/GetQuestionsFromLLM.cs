﻿using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.LLMControllers
{
    public class GetQuestionsFromLLM(IChatGPTRequest chatGPTAPI, SaveAILogController saveAILogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly SaveAILogController _saveAILogController = saveAILogController;

        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync(Book book, int questionsPerSection = 1)
        {
            try
            {
                if (book == null || book.Chapters.Count <= 0) return new APIResponse<Book>
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    Data = new()
                };

                foreach (var chapter in book.Chapters)
                {
                    foreach (var section in chapter.Sections)
                    {
                        var prompt = CreateBookPrompts.GetNewQuestionFromLLMBook(section.Content.Text!, questionsPerSection);
                        var responseLLM = await _chatGPTRequest.ExecuteAsync(prompt) ?? throw new Exception();
                        await _saveAILogController.ExecuteAsync(nameof(ExecuteAsync), responseLLM);
                        var newQuestion = JSONConverter.ConvertToQuestion(responseLLM);

                        Console.WriteLine(newQuestion.Name);
                        Console.WriteLine(newQuestion.Answer);

                        section.Questions.Add(newQuestion);
                    }
                }

                APIResponse<Book> bookAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = book
                };

                return bookAPIResponse;

            }
            catch (Exception ex)
            {
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }
                errorMessage += $" StackTrace: {ex.StackTrace}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<ActionResult<APIResponse<List<Question>>>> ExecuteAsync(string pdfDataPages, int questionsPerSection = 1)
        {
            try
            {
                if (pdfDataPages == null) return new APIResponse<List<Question>>
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    Data = new List<Question>()
                };

                var questions = new List<Question>();
                var prompt = QuestionsPrompts.GetNewQuestionFromPages(pdfDataPages, questionsPerSection);
                var responseLLM = await _chatGPTRequest.ExecuteAsync(prompt) ?? throw new Exception();
                await _saveAILogController.ExecuteAsync(nameof(ExecuteAsync), responseLLM);
                questions = JSONConverter.ConvertToQuestions(responseLLM);
                foreach (var question in questions)
                {
                    Console.WriteLine(question.Name);
                    foreach (var option in question.Options)
                    {
                        Console.WriteLine(option.Name);
                    }
                    Console.WriteLine(question.Hint);
                    Console.WriteLine(question.Resolution);
                }

                APIResponse<List<Question>> questionsAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = questions,
                    Message = ""
                };

                return questionsAPIResponse;

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
