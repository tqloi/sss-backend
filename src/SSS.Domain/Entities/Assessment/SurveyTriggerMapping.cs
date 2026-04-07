namespace SSS.Domain.Entities.Assessment
{
    public class SurveyTriggerMapping
    {
        public long Id { get; set; }

        public long SurveyId { get; set; }

        public string TriggerType { get; set; } = default!;

        public int? MaxAttempts { get; set; }

        public int? CooldownDays { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Survey Survey { get; set; } = default!;

        public SurveyTriggerType SurveyTriggerType { get; set; } = default!;
    }
}
