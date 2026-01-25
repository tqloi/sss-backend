using MediatR;
using SSS.Application.Features.QuizAnswers.Common;

namespace SSS.Application.Features.QuizAnswers.CreateQuizAnswer
{
    public sealed record CreateQuizAnswerCommand(CreateQuizAnswerDto QuizAnswer) 
        : IRequest<CreateQuizAnswerResponse>
    {
    }
}