﻿namespace Quiztle.CoreBusiness.Entities.Quiz.DTO
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = "";
        public Question? Question { get; set; }
        public List<OptionDTO> OptionsDTO { get; private set; }
        private bool isFinished;
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                if (!isFinished)
                {
                    isFinished = value;
                }
            }
        }

        public QuestionDTO(Question question)
        {
            Id = question.Id;
            Question = question;
            OptionsDTO = question.Options?.Select(option => new OptionDTO
            {
                Id = option.Id,
                Name = option.Name,
                IsSelected = false,
                IsCorrect = option.IsCorrect,
            }).ToList() ?? [];
        }

        public void ShuffleOptions()
        {
            if (IsFinished) return;

            Random rng = new();
            int n = OptionsDTO.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = OptionsDTO[k];
                OptionsDTO[k] = OptionsDTO[n];
                OptionsDTO[n] = value;
            }
        }
    }
}
