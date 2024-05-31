namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class OptionDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
