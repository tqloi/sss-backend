using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class QuizAttemptQuestionReviewDto
    {
        public long QuestionId { get; set; }
        public string Prompt { get; set; } = null!;
        public QuizQuestionType Type { get; set; }
        public long? SelectedOptionId { get; set; }
        public List<long> SelectedOptionIds { get; set; } = new();
        public List<string> SelectedOptionTexts { get; set; } = new();
        public string? SelectedTextValue { get; set; }
        public string? SelectedOptionText { get; set; }
        public long? CorrectOptionId { get; set; }
        public List<long> CorrectOptionIds { get; set; } = new();
        public List<string> CorrectOptionTexts { get; set; } = new();
        public string? CorrectTextValue { get; set; }
        public string? CorrectOptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}