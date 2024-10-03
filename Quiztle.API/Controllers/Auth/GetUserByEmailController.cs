using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;

namespace Quiztle.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUserByEmailController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public GetUserByEmailController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> ExecuteAsync(string email)
        {
            try
            {
                // Chama o método do repositório para obter o usuário pelo e-mail
                var user = await _userRepository.GetUserByEmailAsync(email);

                if (user == null)
                {
                    // Retorna uma resposta 404 se o usuário não for encontrado
                    return NotFound($"User with email {email} not found.");
                }

                // Retorna uma resposta de sucesso com o usuário encontrado
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Retorna uma resposta de erro se algo der errado
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
