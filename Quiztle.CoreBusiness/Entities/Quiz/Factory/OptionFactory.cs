using System;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public static class OptionFactory
    {
        public static Option CreateFilledOption()
        {
            return new Option
            {
                Id = Guid.NewGuid(),
                Name = "Sample Option",
                IsCorrect = false,
                Created = DateTime.UtcNow,
                QuestionId = Guid.NewGuid()
            };
        }
    }
}
