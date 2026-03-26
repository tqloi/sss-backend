using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestions
{
    public sealed class CreateQuizQuestionWithOptionsDto
    {
        public long QuizId { get; set; }
        public string Level { get; set; } = null!;
        public string QuestionKey { get; set; } = null!;
        public string Prompt { get; set; } = null!;
        public QuizQuestionType Type { get; set; }
        public decimal ScoreWeight { get; set; }
        public int OrderNo { get; set; }
        public bool IsRequired { get; set; }
        public List<CreateQuizQuestionOptionInputDto> Options { get; set; } = new();
    }

    public sealed class CreateQuizQuestionOptionInputDto
    {
        public string ValueKey { get; set; } = null!;
        public string DisplayText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public decimal? ScoreValue { get; set; }
        public int OrderNo { get; set; }
    }
}
