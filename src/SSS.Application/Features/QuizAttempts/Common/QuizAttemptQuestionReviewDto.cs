namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class QuizAttemptQuestionReviewDto
    {
        public long QuestionId { get; set; }
        public string Prompt { get; set; } = null!;
        public long? SelectedOptionId { get; set; }
        public string? SelectedOptionText { get; set; }
        public long? CorrectOptionId { get; set; }
        public string? CorrectOptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}