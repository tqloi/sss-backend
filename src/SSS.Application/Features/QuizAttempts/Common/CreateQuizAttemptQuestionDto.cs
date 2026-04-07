using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class CreateQuizAttemptQuestionDto
    {
        public long QuestionId { get; set; }
        public string Prompt { get; set; } = null!;
        public QuizQuestionType Type { get; set; }
        public int OrderNo { get; set; }
        public List<CreateQuizAttemptQuestionOptionDto> Options { get; set; } = new List<CreateQuizAttemptQuestionOptionDto>();
    }
}