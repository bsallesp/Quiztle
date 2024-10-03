using Quiztle.CoreBusiness.Log;
using Quiztle.DataContext.Repositories;
using Newtonsoft.Json;

namespace Quiztle.API.Services
{
    public class LogService<T>(AILogRepository aILogRepository)
    {
        private readonly AILogRepository _aILogRepository = aILogRepository;

        public async Task<T> SaveAndReturnLog(T response)
        {
            string logMessage;
            string name = nameof(response);
            try
            {
                logMessage = JsonConvert.SerializeObject(response);
            }
            catch
            {
                logMessage = response!.ToString()!;
            }

            await SaveLogToDatabase(logMessage, name);
            SaveLogToFile(logMessage, name);

            return response;
        }

        private async Task SaveLogToDatabase(string logMessage, string name)
        {
            await _aILogRepository.CreateAILogAsync(new AILog
            {
                Name = name,
                JSON = logMessage
            });
        }

        private void SaveLogToFile(string logMessage, string name)
        {
            string filePath = "log.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + " - " + name + ": " + logMessage);
            }
        }
    }
}
