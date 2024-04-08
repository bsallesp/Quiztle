using BrunoTheBot.API.Controllers.FromLLMToDBControllers;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers.Retrieve
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveSchoolAndTopicsCourseById(SchoolRepository schoolDb) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;


        [HttpPost("RetrieveSchoolAndTopicsCourseById")]
        public async Task<ActionResult<SchoolAPIResponse>> ExecuteAsync([FromBody] int schoolId)
        {
            SchoolAPIResponse schoolAPIResponse = new SchoolAPIResponse();

            try
            {
                schoolAPIResponse = new SchoolAPIResponse {
                    Status = CustomStatusCodes.SuccessStatus,
                    School = await _schoolDb.GetSchoolByIdAsync(schoolId, true) ?? new School()
                };

                return schoolAPIResponse;
            }
            catch(Exception ex)
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