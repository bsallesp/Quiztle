using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.DataContext.DataService.Repository.Quiz
{
    public class TestRepository
    {
        private readonly PostgreQuiztleContext _context;

        public TestRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> RemoveTestById(Guid id)
        {
            try
            {
                EnsureTestNotNull();
                var test = await _context.Tests!.FindAsync(id);
                if (test == null)
                {
                    // Caso o teste não seja encontrado
                    throw new KeyNotFoundException("Test not found");
                }

                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                // Lida com o caso onde o teste não é encontrado
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Lida com qualquer outra exceção
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task CreateTestAsync(Test test)
        {
            try
            {
                EnsureTestNotNull();
                _context.Tests!.Add(test);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateScratchAsync: An exception occurred while creating the test:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Test?> GetTestByIdAsync(Guid testId)
        {
            return await _context.Tests!
                .Include(t => t.Questions)
                .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync(t => t.Id == testId);
        }

        public async Task AddQuestionsToTestAsync(Guid testId, List<Question> questions)
        {
            EnsureTestNotNull();
            var test = await _context.Tests!
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
            {
                throw new KeyNotFoundException($"No test found with ID {testId}");
            }

            foreach (var question in questions)
            {
                test.Questions.Add(question);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Test>> GetAllTestsByPDFDataIdAsync(Guid PDFDataId)
        {
            EnsureTestNotNull();
            var response = await _context.Tests!.Where(p => p.PDFDataId == PDFDataId)!
                .Include(q => q.Questions)
                .ToListAsync();
            return response;
        }

        public async Task AddRandomQuestionsFromScratchesAsync(Guid testId, IEnumerable<Scratch> scratches, int numberOfQuestions)
        {
            try
            {
                // Certifica que o testId é válido
                EnsureTestNotNull();

                // Busca o teste existente no banco de dados
                var test = await _context.Tests!
                    .Include(t => t.Questions)
                    .FirstOrDefaultAsync(t => t.Id == testId);

                if (test == null)
                {
                    throw new KeyNotFoundException($"Test with ID {testId} not found.");
                }

                // Lista para armazenar as perguntas que serão adicionadas ao teste
                var questionsToAdd = new List<Question>();

                // Percorre cada Scratch
                foreach (var scratch in scratches)
                {
                    // Para cada Scratch, percorre seus Drafts
                    foreach (var draft in scratch.Drafts)
                    {
                        // Seleciona X perguntas aleatórias (ou o número de perguntas disponível, se menor que o solicitado)
                        var randomQuestions = draft.Questions
                            .OrderBy(q => Guid.NewGuid()) // Randomiza as perguntas
                            .Take(numberOfQuestions)
                            .ToList();

                        // Adiciona essas perguntas na lista
                        questionsToAdd.AddRange(randomQuestions);
                    }
                }

                // Adiciona as perguntas ao teste
                foreach (var question in questionsToAdd)
                {
                    test.Questions.Add(question);
                }

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding random questions to the test:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task CreateTestsFromScratchesAsync(IEnumerable<Scratch> scratches, int numberOfQuestionsPerTest)
        {
            try
            {
                EnsureTestNotNull();

                // Lista para armazenar os testes a serem criados
                var testsToCreate = new List<Test>();

                // Percorre cada Scratch da lista
                foreach (var scratch in scratches)
                {
                    // Cria um novo teste
                    var newTest = new Test
                    {
                        Name = $"Test from {scratch.Name}", // Nome do teste baseado no Scratch
                        Questions = new List<Question>(), // Inicializa a lista de perguntas
                        Created = DateTime.UtcNow // Ajusta a data de criação ou outros detalhes
                    };

                    // Lista temporária para armazenar todas as perguntas dos Drafts desse Scratch
                    var allQuestionsFromScratch = new List<Question>();

                    // Percorre todos os drafts dentro do Scratch
                    foreach (var draft in scratch.Drafts!)
                    {
                        // Adiciona todas as perguntas do Draft à lista de perguntas do Scratch usando AsNoTracking para não rastrear
                        allQuestionsFromScratch.AddRange(
                            _context.Questions!
                                .Where(q => draft.Questions!.Select(dq => dq.Id).Contains(q.Id))
                                .AsNoTracking()
                                .ToList()
                        );
                    }

                    // Seleciona uma quantidade aleatória de perguntas (ou até 10, se houver menos)
                    var selectedQuestions = allQuestionsFromScratch
                        .OrderBy(q => Guid.NewGuid()) // Randomiza a seleção das perguntas
                        .Take(numberOfQuestionsPerTest)
                        .ToList();

                    // Adiciona essas perguntas ao novo teste
                    newTest.Questions.AddRange(selectedQuestions);

                    // Adiciona o novo teste à lista de testes a serem criados
                    testsToCreate.Add(newTest);
                }

                // Adiciona todos os testes criados ao contexto do banco de dados
                await _context.Tests!.AddRangeAsync(testsToCreate);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating tests from scratches:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<Test>> GetAllTestsAsync()
        {
            EnsureTestNotNull();
            var response = await _context.Tests!
                .Include(q => q.Questions)
                .ToListAsync();
            return response;
        }

        public async Task UpdateTest(Test test)
        {
            EnsureTestNotNull();
            var existingTest = await _context.Tests!.FirstOrDefaultAsync(t => t.Id == test.Id);
            if (existingTest == null)
                throw new Exception("Test with ID: " + test.Id + " not found for update.");
            else
            {
                existingTest.Name = test.Name;
                existingTest.Responses = test.Responses;
                existingTest.Questions = test.Questions;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Test?> GetTestByNameAsync(string name)
        {
            EnsureTestNotNull();
            return await _context.Tests!
                .Include(t => t.Questions)
                .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        private void EnsureTestNotNull()
        {
            if (_context.Tests == null)
            {
                throw new InvalidOperationException("CreateScratchAsync: The Tests DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
