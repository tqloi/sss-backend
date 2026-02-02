using MediatR;

namespace SSS.Application.Features.Content.Roadmap.GetRoadMapByManager
{
    public sealed record GetRoadMapByManagerQuery(
        string ManagerId,
        int PageIndex,
        int PageSize,
        long? SubjectId = null,
        string? Keyword = null,
        string? Status = null,
        int? Version = null,
        bool? IsLatest = null
    ) : IRequest<GetRoadMapByManagerResult>;
}
