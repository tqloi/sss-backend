using MediatR;

namespace SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt
{
    public sealed record GetQuizzesByQuizAttemptQuery(long AttemptId)
        : IRequest<GetQuizzesByQuizAttemptResult>
    {
    }
}
