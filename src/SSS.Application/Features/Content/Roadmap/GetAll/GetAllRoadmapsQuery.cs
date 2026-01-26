using MediatR;

namespace SSS.Application.Features.Content.Roadmap.GetAll
{
    public sealed record GetAllRoadmapsQuery(
        int PageIndex,
        int PageSize,
        long? SubjectId = null,
        string? Q = null,
        string? Status = null
    ) : IRequest<GetAllRoadmapsResult>;
}
