namespace SSS.Domain.Entities.Content;

public class LearningSubject
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public LearningCategory Category { get; set; } = null!;
    public ICollection<Roadmap> Roadmaps { get; set; } = new HashSet<Roadmap>();
}