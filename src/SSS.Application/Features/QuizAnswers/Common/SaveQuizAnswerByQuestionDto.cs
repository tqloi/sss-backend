namespace SSS.Application.Features.QuizAnswers.Common
{
    public sealed class SaveQuizAnswerByQuestionDto
    {
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? AnsweredAt { get; set; }
    }
}