using LecX.Application.Common.Dtos;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public sealed record GetAllQuizzesByStudyPlanModuleIdResponse(PaginatedResponse<QuizDto> Quizzes)
    {
    }
}