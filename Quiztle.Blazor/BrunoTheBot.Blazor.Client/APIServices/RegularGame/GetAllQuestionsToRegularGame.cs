using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.CoreBusiness.Entities.Quiz.DTO;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.RegularGame
{
    public class GetAllQuestionsToRegularGame
    {
        private HttpClient _httpClient;

        public GetAllQuestionsToRegularGame(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QuestionGameDTO>> ExecuteAsync(Guid bookId)
        {
            try
            {
                List<QuestionGameDTO> questions = [];
                var stringResponse = await _httpClient.GetStringAsync("/api/RetrieveBookById/" + bookId);

                APIResponse<Book> bookAPIResponse = JsonSerializer.Deserialize<APIResponse<Book>>(stringResponse)!;

                if (bookAPIResponse.Status != CustomStatusCodes.SuccessStatus) throw new Exception(bookAPIResponse.Status);

                foreach (var chapter in bookAPIResponse.Data.Chapters)
                {
                    foreach (var sectionObj in chapter.Sections)
                    {
                        foreach (var question in sectionObj.Questions)
                        {
                            var questionGameDTO = question.ToQuestionGame();
                            questionGameDTO.ShuffleOptions();
                            questionGameDTO.SetAllOptionsFalse();
                            questions.Add(questionGameDTO);
                        }
                    }
                }
                
                return questions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");

                throw new Exception("An error occurred while processing the book. See inner exception for details.", ex);
            }

        }
    }
}