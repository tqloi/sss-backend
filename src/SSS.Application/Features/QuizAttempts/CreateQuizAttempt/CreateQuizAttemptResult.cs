using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.CreateQuizAttempt
{
    public sealed record CreateQuizAttemptResult(
        QuizAttemptDto QuizAttempt,
        List<CreateQuizAttemptQuestionDto> Questions);
}
