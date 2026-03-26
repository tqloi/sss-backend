using SSS.Application.Common.Dtos;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.CreateQuizAttempt
{
    public sealed record CreateQuizAttemptResult(
        bool Success,
        string Message,
        QuizAttemptDto? Data = null) : GenericResponseRecord<QuizAttemptDto>(Success, Message, Data);
}
