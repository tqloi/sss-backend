using SSS.Application.Features.QuizAttempts.Common;
namespace SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt
{
    public sealed record GetQuizzesByQuizAttemptResult(List<QuizQuestionWithAnswerDto> Questions);
}
