using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapEdges.Update;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.Update
{
    public class UpdateRoadmapEdgeEndpoint(ISender sender)
        : Endpoint<UpdateRoadmapEdgeCommand, UpdateRoadmapEdgeResult>
    {
        public override void Configure()
        {
            Patch("/api/roadmaps/{roadmapId}/edges/{edgeId}");
            Summary(s => s.Summary = "Update roadmap edge");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            UpdateRoadmapEdgeCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
