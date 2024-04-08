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
    public class RetrieveSchoolCourseById(SchoolRepository schoolDb) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;


        [HttpPost("RetrieveSchoolCourseById")]
        public async Task<ActionResult<SchoolAPIResponse>> ExecuteAsync([FromBody] int schoolId)
        {
            SchoolAPIResponse schoolAPIResponse = new SchoolAPIResponse();

            try
            {
                schoolAPIResponse = new SchoolAPIResponse {
                    Status = CustomStatusCodes.SuccessStatus,
                    School = await _schoolDb.GetSchoolByIdAsync(schoolId) ?? new School()
                };

                return schoolAPIResponse;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}