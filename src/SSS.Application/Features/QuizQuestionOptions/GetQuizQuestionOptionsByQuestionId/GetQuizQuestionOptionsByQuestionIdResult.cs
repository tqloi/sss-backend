using SSS.Application.Features.QuizQuestionOptions.Common;

namespace SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId
{
    public sealed record GetQuizQuestionOptionsByQuestionIdResult(List<QuizQuestionOptionDto> QuizQuestionOptionDto)
    {
    }
}