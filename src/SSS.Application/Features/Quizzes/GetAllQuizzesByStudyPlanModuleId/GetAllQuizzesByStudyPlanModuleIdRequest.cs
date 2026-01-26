using MediatR;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public class GetAllQuizzesByStudyPlanModuleIdRequest : IRequest<GetAllQuizzesByStudyPlanModuleIdResponse>
    {
        public long StudyPlanModuleId { get; init; }
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}