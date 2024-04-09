using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Net.Http.Json;
using System.Text.Json;

namespace BrunoTheBot.Blazor.APIServices
{
    public class GetAllSchoolsService
    {
        private readonly HttpClient _httpClient;

        public GetAllSchoolsService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("http://localhost:5044");
        }

        public async Task<SchoolsAPIResponse> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllSchools/GetAllSchools");

                SchoolsAPIResponse schoolsAPIResponse = JsonSerializer.Deserialize<SchoolsAPIResponse>(stringResponse)!;

                return schoolsAPIResponse;
            }
            catch
            {
                return new SchoolsAPIResponse
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Schools = null
                };
            }
        }
    }
}
