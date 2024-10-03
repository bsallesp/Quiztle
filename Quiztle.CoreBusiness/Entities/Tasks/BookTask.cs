using Quiztle.CoreBusiness;

public class BookTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = "";

    // Propriedade de navegação
    public User? User { get; set; }

    // Foreign Key
    public Guid? UserId { get; set; }

    public byte Status { get; set; } = 0;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
