namespace SSS.Domain.Entities.Content;

public class LearningCategory
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public virtual ICollection<LearningSubject> Subjects { get; set; } = new HashSet<LearningSubject>();
}