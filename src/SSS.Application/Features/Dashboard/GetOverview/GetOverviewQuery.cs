using MediatR;

namespace SSS.Application.Features.Dashboard.GetOverview
{
    public class GetOverviewQuery : IRequest<GetOverviewResult>
    {
        public string UserId { get; set; } = null!;
        public long StudyPlanId { get; set; }
    }

    public class GetOverviewResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public OverviewStudyPlanDto Data { get; set; } = null!;
    }
}
