using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Quiztle.API.Prompts;
using Quiztle.API.Services;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.CoreBusiness.Utils;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;
using System.Xml.Linq;


namespace Quiztle.API.BackgroundTasks.Questions
{
    public class BGQuestions
    {
        private readonly ILLMRequest _llmRequest;
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AILogRepository _aILogRepository;

        public BGQuestions(ILLMRequest ollamaRequest,
            PDFDataRepository pDFDataRepository,
            QuestionRepository questionRepository,
            AILogRepository aILogRepository
            )
        {
            _llmRequest = ollamaRequest;
            _pDFDataRepository = pDFDataRepository;
            _questionRepository = questionRepository;
            _aILogRepository = aILogRepository;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            try
            {
                Guid guid = new Guid("511c65f6-4200-4700-a135-9dd5cc468a54");
                var _pdfDataAZ900 = await _pDFDataRepository.GetPDFDataByIdAsync(guid);

                // Descobrir o total de páginas no PDF
                int totalPages = _pdfDataAZ900.Pages.Count;

                // Gerar um número aleatório para selecionar uma página
                int randomPageIndex = new Random().Next(0, totalPages);

                // Atribuir o conteúdo da página aleatória
                var llmInput = QuestionsPrompts.GetNewQuestionFromPages(_pdfDataAZ900.Pages[randomPageIndex].Content, 3);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    Name = "BGQuestions",
                    JSON = llmInput,
                    Created = DateTime.UtcNow
                });

                var llmResult = await _llmRequest.ExecuteAsync(llmInput);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    Name = "BGQuestions",
                    JSON = llmResult,
                    Created = DateTime.UtcNow
                });

                Console.WriteLine(llmResult);

                JObject jsonObject = JObject.Parse(llmResult);
                var questionsToken = jsonObject["Questions"];
                if (questionsToken == null) throw new ArgumentException("No 'Questions' found in JSON.");
                var questions = questionsToken.ToObject<List<Question>>();


                Console.WriteLine("Total questions in json: " + questions.Count);

                foreach(var question in questions) await _questionRepository.CreateQuestionAsync(question);

                Console.WriteLine(llmResult);

                return new JsonResult(llmInput) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                var error = $"Error in BGQuestions/ExecuteAsync: {ex.Message}";
                Console.WriteLine(error);
                return new ObjectResult(error) { StatusCode = 500 };
            }
        }

    }
}