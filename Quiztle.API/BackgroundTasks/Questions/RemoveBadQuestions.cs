using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.BackgroundTasks.Questions
{
    public class RemoveDuplicates
    {
        private readonly QuestionRepository _questionRepository;

        public RemoveDuplicates(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        public async Task ExecuteAsync()
        {
            var allQuestions = await _questionRepository.GetAllQuestionsAsync();

            var duplicateGroups = allQuestions
                .Where(q => q.Verified)
                .GroupBy(q => q.Name)
                .Where(g => g.Count() > 1);

            foreach (var group in duplicateGroups)
            {
                // Keep the first entry and delete the rest
                var firstEntry = group.First();
                var duplicates = group.Skip(1);

                foreach (var duplicate in duplicates)
                {
                    // Use DeleteQuestionAsync to remove the duplicate entry
                    await _questionRepository.DeleteQuestionAsync(duplicate.Id);
                }
            }

            Console.WriteLine("Duplicate questions removed.");
        }
    }
}
