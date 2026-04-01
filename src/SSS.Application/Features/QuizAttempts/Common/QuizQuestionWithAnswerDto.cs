namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class QuizQuestionWithAnswerDto
    {
        public long QuestionId { get; set; }
        public string Prompt { get; set; } = null!;
        public int OrderNo { get; set; }
        public List<QuizOptionWithAnswerDto> Options { get; set; } = new();
        public long? SelectedOptionId { get; set; }
    }

    public sealed class QuizOptionWithAnswerDto
    {
        public long OptionId { get; set; }
        public string ValueKey { get; set; } = null!;
        public string DisplayText { get; set; } = null!;
        public int OrderNo { get; set; }
    }
}
