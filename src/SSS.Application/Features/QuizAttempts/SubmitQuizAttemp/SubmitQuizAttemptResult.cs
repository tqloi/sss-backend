using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.SubmitQuizAttemp
{
    public sealed record SubmitQuizAttemptResult(
        QuizAttemptDto QuizAttempt,
        List<QuizAttemptQuestionReviewDto> Questions);
}
