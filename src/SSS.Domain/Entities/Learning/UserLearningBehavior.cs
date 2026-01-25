using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Learning
{
    public class UserLearningBehavior
    {
        public long Id { get; set; }

        public string UserId { get; set; } = default!;

        // Time & availability
        public string? AvailableDaysJson { get; set; }
        public string? PreferredTimeBlocksJson { get; set; }
        public int? SessionLengthPrefMinutes { get; set; }

        // Learning style weights
        public decimal WVisual { get; set; }
        public decimal WReading { get; set; }
        public decimal WPractice { get; set; }

        // Self-reported discipline & difficulty
        public DisciplineType? DisciplineType { get; set; }
        public string? CommonDifficultiesJson { get; set; }

        public long SourceSurveyResponseId { get; set; } // BẮT BUỘC
        public int SnapshotVersion { get; set; } // tăng rất hiếm
        public DateTime SnapshotAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
