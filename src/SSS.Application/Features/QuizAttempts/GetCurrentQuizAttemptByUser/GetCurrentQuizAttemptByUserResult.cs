using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.GetCurrentQuizAttemptByUser
{
    public sealed record GetCurrentQuizAttemptByUserResult(QuizAttemptDto? QuizAttempt);
}
