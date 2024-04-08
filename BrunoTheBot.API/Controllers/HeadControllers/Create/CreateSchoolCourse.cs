using BrunoTheBot.API.Controllers.FromLLMToDBControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;


namespace BrunoTheBot.API.Controllers.HeadControllers.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateSchoolCourse(SchoolRepository schoolDb, FromLLMToTopicClasses topicClassesFromLLM, FromLLMToContent fromLLMToContent) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;
        private readonly FromLLMToTopicClasses _fromLLMToTopicClasses = topicClassesFromLLM;
        private readonly FromLLMToContent _fromLLMToContent = fromLLMToContent;

        [HttpPost("CreateSchoolFullCourse")]
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