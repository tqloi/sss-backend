using MediatR;

namespace SSS.Application.Features.QuizAttempts.GetQuizAttemptById
{
    public sealed record GetQuizAttemptByIdQuery(long Id) 
        : IRequest<GetQuizAttemptByIdResult>
    {
    }
}