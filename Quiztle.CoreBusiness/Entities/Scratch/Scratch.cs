namespace Quiztle.CoreBusiness.Entities.Scratch
{
    public class Scratch
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
    }
}