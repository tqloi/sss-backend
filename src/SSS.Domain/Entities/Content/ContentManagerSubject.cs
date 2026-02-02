using SSS.Domain.Entities.Identity;

namespace SSS.Domain.Entities.Content
{
    public class ContentManagerSubject
    {
        public long Id { get; set; }

        public string ContentManagerId { get; set; } = null!;
        public long SubjectId { get; set; }

        public string? AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }

        // Navigation
        public User ContentManager { get; set; } = null!;
        public User? AssignedByUser { get; set; }
        public LearningSubject Subject { get; set; } = null!;
    }
}
