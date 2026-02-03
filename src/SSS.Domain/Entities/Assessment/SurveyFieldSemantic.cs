namespace SSS.Domain.Entities.Assessment
{
    public class SurveyFieldSemantic
    {
        public long Id { get; set; }

        public long SurveyQuestionId { get; set; }

        public string DimensionCode { get; set; } = default!;

        public string Evaluates { get; set; } = default!;

        public string? AIHint { get; set; }

        public double? Weight { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public SurveyQuestion SurveyQuestion { get; set; } = default!;
    }
}
