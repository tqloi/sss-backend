using SSS.Application.Common.Dtos;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.GetAllQuizAttempts
{
    public sealed record GetAllQuizAttemptsResult(PaginatedResponse<QuizAttemptDto> QuizzAttempts)
    {
    }
}