using SSS.Application.Features.QuizQuestions.Common;

namespace SSS.Application.Features.QuizQuestions.GetAllQuizQuestionsByQuizId
{
    public sealed record GetAllQuizQuestionsByQuizIdResult(List<QuizQuestionDto> QuizQuestionDtos)
    {
    }
}