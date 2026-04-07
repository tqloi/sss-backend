using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Learning
{
    public class UserLearningTarget
    {
        public long Id { get; set; }

        public string UserId { get; set; } = default!;
        public long RoadmapId { get; set; }

        public int ProfileVersion { get; set; }

        public string TargetRole { get; set; } = default!;
        public string CurrentLevel { get; set; } = default!;
        public int? TargetDeadlineMonths { get; set; }

        public string? GoalDescription { get; set; }

        public long SourceSurveyResponseId { get; set; } // BẮT BUỘC
        public DateTime SnapshotAt { get; set; }

        // active / archived / completed
        public TargetStatus Status { get; set; } = TargetStatus.active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
