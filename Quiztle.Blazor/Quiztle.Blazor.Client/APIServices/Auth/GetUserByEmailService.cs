using Quiztle.CoreBusiness;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using System.Net.Http.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class GetUserByEmailService
    {
        private readonly HttpClient _httpClient;

        public GetUserByEmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<User>> ExecuteAsync(string email)
        {
            try
            {
                var url = $"api/GetUserByEmail/{email}";

                // Faz uma requisição GET para obter o usuário pelo e-mail
                var stringResponse = await _httpClient.GetAsync(url);

                if (stringResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new APIResponse<User>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new User(),
                        Message = "Email not found."
                    };
                }

                if (!stringResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetUserByEmailService:");

                    return new APIResponse<User>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new User(),
                        Message = "ERROR IN GetAsync - GetUserByEmailService."
                    };
                }

                var user = await stringResponse.Content.ReadFromJsonAsync<User>();

                // Retorna uma resposta de sucesso
                return new APIResponse<User>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = user ?? new User(),
                    Message = "User retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.Data.ToString());

                // Retorna uma resposta de erro caso uma exceção seja lançada
                return new APIResponse<User>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new User(),
                    Message = ex.Message + " ERROR IN GetAsync - GetUserByEmailService " + ex.Data.ToString()
                };
            }
        }
    }
}
