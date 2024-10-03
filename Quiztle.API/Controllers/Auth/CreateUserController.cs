using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness;
using Quiztle.CoreBusiness.Log;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.Repositories;


namespace Quiztle.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AILogRepository _aILogRepository;

        public CreateUserController(UserRepository userRepository, AILogRepository aILogRepository)
        {
            _userRepository = userRepository;
            _aILogRepository = aILogRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] User user)
        {
            try
            {
                var aiLog = new AILog
                {
                    Name = "CreateUserController: " + user.Email

                };
                await _aILogRepository.CreateAILogAsync(aiLog);
                

                // Gera um GUID para o novo usuário
                var userGuid = Guid.NewGuid();

                // Cria um novo objeto User com os valores fornecidos
                User newUser = new()
                {
                    Id = userGuid,
                    Name = user.Name,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = user.Password,
                    Role = user.Role,
                    Claims = user.Claims,
                };

                // Chama o método do repositório para salvar o usuário
                await _userRepository.CreateUserAsync(newUser);

                // Retorna uma resposta de sucesso com o usuário criado
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                // Retorna uma resposta de erro se algo der errado
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
