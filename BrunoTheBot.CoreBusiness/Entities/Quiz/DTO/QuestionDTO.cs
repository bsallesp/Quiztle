namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public Question? Question { get; set; }
        public List<OptionDTO> OptionsDTO { get; private set; }

        public QuestionDTO(Question question)
        {
            Id = question.Id;
            Question = question;

            OptionsDTO = question.Options?.Select(option => new OptionDTO
            {
                Id = option.Id,
                Name = option.Name,
                IsSelected = false
            }).ToList() ?? [];
        }
    }    
}
