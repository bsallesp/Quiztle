namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class QuestionShotDTO
    {
        public required Question? Question { get; set; }
        public required List<ShotDTO> Shots { get; set; }
    }
}
