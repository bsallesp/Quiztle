using BrunoTheBot.CoreBusiness.Entities.Quiz.DTO;

namespace BrunoTheBot.CoreBusiness.Entities.Exam
{
    public class RegularExam
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<QuestionGameDTO> Questions { get; set; } = [];
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public int TotalQuestionsWithTrueAnswer()
        {
            // Inicializa um contador para o total de perguntas com a chave "Answer" definida como verdadeira
            int total = 0;

            // Itera sobre cada QuestionGameDTO na lista Questions
            foreach (var question in Questions)
            {
                // Verifica se a opção com chave "Answer" está definida como verdadeira e incrementa o contador, se verdadeiro
                if (question.Options.TryGetValue("Answer", out var option) && option.Item1)
                {
                    total++;
                }
            }

            // Retorna o total de perguntas com a chave "Answer" definida como verdadeira
            return total;
        }
    }
}
