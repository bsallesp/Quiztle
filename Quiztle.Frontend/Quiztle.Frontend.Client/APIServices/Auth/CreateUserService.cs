﻿using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness;
using System.Net.Http.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class CreateUserService
    {
        private readonly HttpClient _httpClient;

        public CreateUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<User>> ExecuteAsync(User user)
        {
            try
            {
                var url = "api/CreateUser/";

                // Envia o objeto User como JSON via POST
                var stringResponse = await _httpClient.PostAsJsonAsync(url, user);

                if (!stringResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in CreateUserService:");

                    // Retorna um objeto APIResponse indicando falha
                    return new APIResponse<User>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new User(),
                        Message = "ERROR IN PostAsJsonAsync - CreateUserService."
                    };
                }

                // Retorna uma resposta de sucesso
                return new APIResponse<User>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = user,
                    Message = "User created successfully."
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
                    Message = ex.Message + " ERROR IN PostAsJsonAsync - CreateUserService " + ex.Data.ToString()
                };
            }
        }
    }
}
