using SSS.Application.Common.Dtos;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByRoadmapNodeId
{
    public sealed record GetAllQuizzesByRoadmapNodeIdResult(PaginatedResponse<QuizDto> Quizzes)
    {
    }
}