using MediatR;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByRoadmapNodeId
{
    public class GetAllQuizzesByRoadmapNodeIdQuery : IRequest<GetAllQuizzesByRoadmapNodeIdResult>
    {
        public long RoadmapNodeId { get; init; }
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}