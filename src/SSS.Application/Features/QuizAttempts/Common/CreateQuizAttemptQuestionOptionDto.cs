namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class CreateQuizAttemptQuestionOptionDto
    {
        public long OptionId { get; set; }
        public string ValueKey { get; set; } = null!;
        public string DisplayText { get; set; } = null!;
        public int OrderNo { get; set; }
    }
}