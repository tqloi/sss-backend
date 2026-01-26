using SSS.Application.Common.Dtos;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.GetAllQuizzes
{
    public sealed record GetAllQuizzesResult(PaginatedResponse<QuizDto> Quizzes)
    {
    }
}