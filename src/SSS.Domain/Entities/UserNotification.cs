using System;

namespace SSS.Domain.Entities;

public class UserNotification
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? RelatedType { get; set; }
    public long? RelatedId { get; set; }
    public string? RelatedSessionId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public StudySession? RelatedSession { get; set; }
}