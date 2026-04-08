using MediatR;

namespace SSS.Application.Features.Content.Roadmap.GetAll
{
    public sealed record GetAllRoadmapsQuery(
        int PageIndex,
        int PageSize,
        long? CategoryId = null,
        long? SubjectId = null,
        string? Q = null,
        string? Status = null,
        int? Version = null,
        bool? IsLatest = null
    ) : IRequest<GetAllRoadmapsResult>;
}
