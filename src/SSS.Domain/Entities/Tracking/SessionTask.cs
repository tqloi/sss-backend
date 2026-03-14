using SSS.Domain.Entities.Planning;

namespace SSS.Domain.Entities.Tracking;

public class SessionTask
{
    public long Id { get; set; }
    public string StudySessionId { get; set; } = null!;
    public long TaskId { get; set; }
    public string Status { get; set; } = "INCOMPLETE"; // INCOMPLETE, COMPLETED, SKIPPED
    public DateTime? StartTimeUtc { get; set; }
    public DateTime? EndTimeUtc { get; set; }

    // Navigation properties
    public virtual StudySession StudySession { get; set; } = null!;
    public virtual TaskItem TaskItem { get; set; } = null!;
}
