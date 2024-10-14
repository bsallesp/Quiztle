using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Scratch;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task CreateTestAsync(Test test, List<Guid> questionIds)
        {
            try
            {
                EnsureTestNotNull();

                // Buscar as perguntas existentes no banco de dados usando os Ids fornecidos
                var existingQuestions = await _context.Questions!
                    .Where(q => questionIds.Contains(q.Id)).AsNoTracking()
                    .ToListAsync();

                if (existingQuestions.Count != questionIds.Count)
                {
                    throw new InvalidOperationException("Uma ou mais perguntas fornecidas não foram encontradas.");
                }

                // Para cada questão existente, garantimos que o EF Core não esteja rastreando duplicadamente
                foreach (var question in existingQuestions)
                {
                    // Detach a questão se já estiver sendo rastreada no contexto
                    if (_context.Entry(question).State == EntityState.Detached)
                    {
                        _context.Attach(question);
                    }

                    test.TestQuestions.Add(new TestQuestion
                    {
                        Test = test,
                        Question = question,
                        TestId = test.Id,
                        QuestionId = question.Id
                    });
                }

                // Adicionar o teste ao contexto
                _context.Tests!.Add(test);

                // Salvar as mudanças
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateTestAsync: An exception occurred while creating the test:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            EnsureTestNotNull();
            await _context.SaveChangesAsync();
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

            var existingQuestionIds = test.Questions.Select(q => q.Id).ToHashSet();

            foreach (var question in questions)
            {
                if (!existingQuestionIds.Contains(question.Id))
                {
                    test.Questions.Add(question);
                }
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

                var testsToCreate = new List<Test>();

                foreach (var scratch in scratches)
                {
                    var newTest = new Test
                    {
                        Name = $"Test from {scratch.Name}",
                        Questions = new List<Question>(),
                        Created = DateTime.UtcNow
                    };

                    var allQuestionsFromScratch = new List<Question>();

                    foreach (var draft in scratch.Drafts ?? Enumerable.Empty<Draft>())
                    {
                        // Fetch questions for each draft efficiently
                        var questionsFromDraft = await _context.Questions!
                            .Where(q => draft.Questions!.Select(dq => dq.Id).Contains(q.Id))
                            .AsNoTracking()
                            .ToListAsync();

                        allQuestionsFromScratch.AddRange(questionsFromDraft);
                    }

                    var selectedQuestions = allQuestionsFromScratch
                        .OrderBy(q => Guid.NewGuid()) // Randomize
                        .Take(numberOfQuestionsPerTest)
                        .DistinctBy(q => q.Id) // Ensure no duplicate questions
                        .ToList();

                    newTest.Questions.AddRange(selectedQuestions);

                    testsToCreate.Add(newTest);
                }

                // Bulk insert tests
                await _context.Tests!.AddRangeAsync(testsToCreate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating tests from scratches:");
                Console.WriteLine(ex.ToString());
                // Optionally, use a logging framework here
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
            var existingTest = await _context.Tests!
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == test.Id);

            if (existingTest == null)
                throw new Exception("Test with ID: " + test.Id + " not found for update.");
            else
            {
                existingTest.Name = test.Name;
                existingTest.Responses = test.Responses;

                // Clear existing questions and add new ones
                existingTest.Questions.Clear();
                existingTest.Questions.AddRange(test.Questions);
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
