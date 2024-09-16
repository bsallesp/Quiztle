using System;
using System.Collections.Generic;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public static class QuestionFactory
    {
        public static Question CreateFilledQuestion()
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                Name = "Sample Question",
                Hint = "Sample Hint",
                Resolution = "Sample Resolution",
                Created = DateTime.UtcNow,
                DraftId = Guid.NewGuid(),
                Verified = true,
                Rate = 5
            };

            question.Options.Add(OptionFactory.CreateFilledOption());
            question.Options.Add(OptionFactory.CreateFilledOption());
            question.Options.Add(OptionFactory.CreateFilledOption());

            return question;
        }
    }
}
