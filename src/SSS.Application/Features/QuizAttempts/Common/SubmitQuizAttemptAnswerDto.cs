namespace SSS.Application.Features.QuizAttempts.Common
{
    public class SubmitQuizAttemptAnswerDto
    {
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
    }
}
