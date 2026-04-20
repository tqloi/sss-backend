using SSS.Domain.Entities.Identity;
using System;

namespace SSS.Domain.Entities.Content
{
    public class Review
    {
        public long Id { get; set; }
        public long RoadmapId { get; set; }
        public string? ReviewerId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Roadmap Roadmap { get; set; } = null!;
        // Optionally, add navigation to Reviewer if needed
        public User Reviewer { get; set; } = null!;
    }
}