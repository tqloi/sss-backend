using LecX.Application.Common.Dtos;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.GetAllQuizNode
{
    public sealed record GetAllQuizzesResponse(PaginatedResponse<QuizDto> Quizzes)
    {
    }
}