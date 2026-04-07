namespace SSS.Application.Features.QuizAnswers.Common
{
    public class CreateQuizAnswerDto
    {
        
        public long AttemptId { get; set; }
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
    }
}
