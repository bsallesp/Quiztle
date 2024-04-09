using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.API.Controllers.HeadControllers.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateSchoolCourse(SchoolRepository schoolDb,
        FromLLMToTopicClasses topicClassesFromLLM,
        FromLLMToContent fromLLMToContent,
        FromLLMToQuestions fromLLMToQuestions
        ) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;
        private readonly FromLLMToTopicClasses _fromLLMToTopicClasses = topicClassesFromLLM;
        private readonly FromLLMToContent _fromLLMToContent = fromLLMToContent;
        private readonly FromLLMToQuestions _fromLLMToQuestions = fromLLMToQuestions;

        [HttpPost("CreateSchoolCourse")]
        public async Task<ActionResult<SchoolAPIResponse>> ExecuteAsync([FromBody] string schoolName, int topicsAmount = 5, int sectionsAmount = 5)
        {
            try
            {
                var topicClassesAPIResponse = await _fromLLMToTopicClasses.GetNewTopicClassesFromLLM(schoolName, topicsAmount);
                var newTopicClasses = topicClassesAPIResponse.Value?.TopicClassesAquired;

                School newSchool = new()
                {
                    Name = schoolName,
                    Topics = newTopicClasses!
                };

                var schoolAPIResponse = await _fromLLMToTopicClasses.GetNewSectionsFromLLM(newSchool, sectionsAmount);
                newSchool = schoolAPIResponse.Value!.School;
                var contentAPIResponse = await _fromLLMToContent.GetFullContentFromLLM(newSchool);
                if (contentAPIResponse.Value!.Status == CustomStatusCodes.ErrorStatus) throw new Exception();
                newSchool = contentAPIResponse.Value.School;
                
                var questionsContentAPIResponse = await _fromLLMToQuestions.GetFullNewQuestionsGroupFromLLM(newSchool, 1);
                if (questionsContentAPIResponse.Value!.Status == CustomStatusCodes.ErrorStatus) throw new Exception();
                newSchool = questionsContentAPIResponse.Value.School;

                await _schoolDb.CreateSchoolAsync(newSchool);

                return schoolAPIResponse;
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