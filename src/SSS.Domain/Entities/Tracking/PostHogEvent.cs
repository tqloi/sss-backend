using System;

namespace SSS.Domain.Entities.Tracking
{
    public class PostHogEvent
    {
        public string? EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime? OccurredAt { get; set; }
        public string? UserId { get; set; }
        public string? DistinctId { get; set; }
        public string? SessionId { get; set; }
        public string? PostHogSessionId { get; set; }
        public string? StudyPlanId { get; set; }
        public string? NodeId { get; set; }
        public string? TaskId { get; set; }
        public string? ContentId { get; set; }
        public PostHogEventProperties? Properties { get; set; }
        public DateTime ReceivedAt { get; set; }
    }

    public class PostHogEventProperties
    {
        public string? Subject { get; set; }
        public string? Topic { get; set; }
        public double? DurationMinutes { get; set; }
        public bool? IsCorrect { get; set; }
        public double? TimeSpentSeconds { get; set; }
        public double? CorrectRate { get; set; }
        public string? CurrentUrl { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
    }
}
