using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.BackgroundTasks.Questions
{
    public class RemoveBadQuestions
    {
        private readonly QuestionRepository _questionRepository;

        public RemoveBadQuestions(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        public async Task ExecuteAsync()
        {
            // Obter todas as perguntas
            var allQuestions = await _questionRepository.GetAllQuestionsAsync();

            // Filtrar perguntas com rate menor que 3
            var badQuestions = allQuestions.Where(q => q.Rate < 1).Where(v => v.Verified).ToList();

            // Remover perguntas ruins
            foreach (var question in badQuestions)
            {
                Console.WriteLine("Deleting question: " + question.Id + "with score: " + question.Rate.ToString());
                // Use DeleteQuestionAsync para remover a pergunta
                await _questionRepository.DeleteQuestionAsync(question.Id);
            }

            Console.WriteLine("Questions with rate less than 3 removed.");
        }
    }
}
