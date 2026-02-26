namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    /// <summary>
    /// Represents whether there is a pending survey for a given trigger type and user.
    /// </summary>
    public sealed record GetPendingTriggerSurveyResult
    {
        /// <summary>True if there is a survey the user should complete for this trigger.</summary>
        public bool HasPendingSurvey { get; init; }

        /// <summary>The survey id to take (null if none pending).</summary>
        public long? SurveyId { get; init; }

        /// <summary>The survey code used for navigation (null if none pending).</summary>
        public string? SurveyCode { get; init; }

        /// <summary>The survey title (null if none pending).</summary>
        public string? SurveyTitle { get; init; }

        /// <summary>The trigger type that caused this result.</summary>
        public string TriggerType { get; init; } = default!;

        /// <summary>Number of times user has already completed this survey.</summary>
        public int CompletedAttempts { get; init; }

        /// <summary>Max allowed attempts (null = unlimited).</summary>
        public int? MaxAttempts { get; init; }

        /// <summary>Cooldown days configured (null = no cooldown).</summary>
        public int? CooldownDays { get; init; }
    }
}
