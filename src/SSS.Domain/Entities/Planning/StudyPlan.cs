using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Content;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Planning;

public class StudyPlan
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public long RoadmapId { get; set; }
    public int? ProfileVersion { get; set; }
    public StudyPlanStatus? Status { get; set; }
    public StudyPlanStrategy? Strategy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Roadmap Roadmap { get; set; } = null!;
    public ICollection<StudyPlanModule> Modules { get; set; } = new HashSet<StudyPlanModule>();
    public ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}