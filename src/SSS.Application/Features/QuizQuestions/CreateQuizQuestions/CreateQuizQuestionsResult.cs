using SSS.Application.Features.QuizQuestions.Common;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestions
{
    public sealed record CreateQuizQuestionsResult(List<CreateQuizQuestionWithOptionsDto> CreateQuizQuestionDtos);
}
