using SSS.Application.Features.QuizQuestions.CreateQuizQuestions;

namespace SSS.Application.Features.AI.CreateAiQuizQuestions
{
    public sealed class CreateAiQuizQuestionsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<CreateQuizQuestionWithOptionsDto> Questions { get; set; } = new();
    }
}
