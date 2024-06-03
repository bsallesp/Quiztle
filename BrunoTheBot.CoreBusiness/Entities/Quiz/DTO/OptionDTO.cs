namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class OptionDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }

        public Shot ConvertToShot(Guid responseId)
        {
            return new Shot
            {
                Id = Guid.NewGuid(),
                OptionId = Id,
                Created = DateTime.UtcNow,
                ResponseId = responseId
            };
        }
    }
}
