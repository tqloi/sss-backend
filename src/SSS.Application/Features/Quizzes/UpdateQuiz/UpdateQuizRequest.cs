using MediatR;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.UpdateQuizNode
{
    public sealed record UpdateQuizRequest(long Id, UpdateQuizDto UpdateQuizNodeDto) 
        : IRequest<UpdateQuizResponse>;
}