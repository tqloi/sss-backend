using MediatR;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.SubmitQuizAttemp
{
    public sealed record SubmitQuizAttemptCommand(SubmitQuizAttempDto SubmitQuizAttempt)
        : IRequest<SubmitQuizAttemptResult>;
}
