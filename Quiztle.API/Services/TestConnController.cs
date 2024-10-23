using Quiztle.DataContext;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Quiztle.API.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConnController : ControllerBase
    {
        private readonly PostgreQuiztleContext _context;

        public TestConnController(PostgreQuiztleContext context)
        {
            _context = context;

            try
            {
                Console.WriteLine("Tentando conectar ao banco de dados...");
                Console.WriteLine(_context.Database.CanConnect() ? "Conexão bem-sucedida!" : "Não foi possível conectar ao banco de dados.");
                Console.WriteLine(_context.Database.EnsureCreated() ? "Criado" : "Nao foi criado");
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Erro ao conectar ao banco de dados:");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado:");
                Console.WriteLine(ex.Message);
            }
        }

        [HttpPost("TestConnection")]
        public ActionResult<string> TestConnection()
        {
            try
            {
                Console.WriteLine("Tentando conectar ao banco de dados...");
                if (_context.Database.CanConnect())
                {
                    Console.WriteLine("Teste de conexão bem-sucedido");
                    return Ok("Teste de conexão bem-sucedido!");
                }
                else
                {
                    Console.WriteLine("Erro ao tentar conectar no banco");
                    return BadRequest("Erro ao tentar conectar no banco!");
                }
                
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Erro ao conectar ao banco de dados:");
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao conectar ao banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro inesperado:");
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro inesperado ao conectar ao banco de dados.");
            }
        }
    }
}
