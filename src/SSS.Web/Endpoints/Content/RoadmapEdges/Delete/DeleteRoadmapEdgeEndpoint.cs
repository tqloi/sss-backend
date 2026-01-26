using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapEdges.Delete;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.Delete
{
    public class DeleteRoadmapEdgeEndpoint(ISender sender)
        : Endpoint<DeleteRoadmapEdgeCommand, DeleteRoadmapEdgeResult>
    {
        public override void Configure()
        {
            Delete("/api/roadmaps/{roadmapId}/edges/{edgeId}");
            Summary(s => s.Summary = "Delete roadmap edge");
            Description(d => d.WithTags("RoadmapEdges"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteRoadmapEdgeCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
