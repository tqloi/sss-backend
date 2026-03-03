using MediatR;

namespace SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId
{
    public sealed record GetQuizQuestionOptionsByQuestionIdQuery(long QuestionId)
        : IRequest<GetQuizQuestionOptionsByQuestionIdResult>
    {
    }
}