using MediatR;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.CreateQuizAttempt
{
    public sealed record CreateQuizAttemptCommand(CreateQuizAttemptDto CreateQuizAttempt)
        : IRequest<CreateQuizAttemptResult>;
}
