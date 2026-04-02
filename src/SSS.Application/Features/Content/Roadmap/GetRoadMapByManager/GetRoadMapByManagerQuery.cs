using MediatR;
using SSS.Domain.Enums;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Content.Roadmap.GetRoadMapByManager
{
    public sealed record GetRoadMapByManagerQuery(
        [property: JsonIgnore] string ManagerId,
        int PageIndex,
        int PageSize,
        long? SubjectId = null,
        string? Keyword = null,
        RoadmapStatus? Status = null,
        int? Version = null,
        bool? IsLatest = null
    ) : IRequest<GetRoadMapByManagerResult>;
}
