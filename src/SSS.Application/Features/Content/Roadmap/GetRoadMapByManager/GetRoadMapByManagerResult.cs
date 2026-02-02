using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.Roadmap.GetRoadMapByManager
{
    public sealed record GetRoadMapByManagerResult(
        PaginatedResponse<RoadmapListItemDTO> Roadmaps
    );
}
