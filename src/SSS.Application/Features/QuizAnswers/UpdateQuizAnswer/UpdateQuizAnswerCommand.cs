using MediatR;
using SSS.Application.Features.QuizAnswers.Common;

namespace SSS.Application.Features.QuizAnswers.UpdateQuizAnswer
{
    public sealed record UpdateQuizAnswerCommand(long Id, UpdateQuizAnswerDto UpdateQuizAnswerDto)
        : IRequest<UpdateQuizAnswerResponse>
    {
    }
}