using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateQuestions(SchoolRepository schoolRepository, FromLLMToQuestions fromLLMToQuestions) : ControllerBase
    {
        private readonly SchoolRepository _schoolRepository = schoolRepository;
        private readonly FromLLMToQuestions _fromLLMToQuestions = fromLLMToQuestions;

        [HttpPost("CreateQuestions")]
        public async Task<ActionResult<SchoolAPIResponse>> ExecuteAsync([FromBody] School school)
        {
            try
            {
                var schoolAPIResponse = await _fromLLMToQuestions.GetFullNewQuestionsGroupFromLLM(school, 1);
                if (schoolAPIResponse.Value!.Status != CustomStatusCodes.SuccessStatus) throw new Exception(schoolAPIResponse.Value!.Status);

                await _schoolRepository.UpdateSchoolAsync(school);
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
            
            return new SchoolAPIResponse();
        }
    }
}
