using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.Roadmap.GetAll
{
    public sealed record GetAllRoadmapsResult(
        PaginatedResponse<RoadmapListItemDTO> Roadmaps
    );
}
