namespace Quiztle.CoreBusiness.Entities.Scratch
{
    public class Scratch
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Draft>? Drafts { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;

        public Scratch() => Drafts = [];
    }
}