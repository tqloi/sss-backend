using MediatR;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.UpdateQuizNode
{
    public sealed record UpdateQuizCommand(long Id, UpdateQuizDto UpdateQuizNodeDto) 
        : IRequest<UpdateQuizResult>;
}