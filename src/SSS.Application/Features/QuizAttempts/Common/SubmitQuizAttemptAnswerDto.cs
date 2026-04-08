namespace SSS.Application.Features.QuizAttempts.Common
{
    public class SubmitQuizAttemptAnswerDto
    {
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public List<long> OptionIds { get; set; } = new List<long>();
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
    }
}
