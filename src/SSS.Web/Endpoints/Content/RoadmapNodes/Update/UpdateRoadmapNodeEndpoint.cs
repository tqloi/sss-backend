using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapNodes.Update;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.Update
{
    public class UpdateRoadmapNodeEndpoint(ISender sender)
        : Endpoint<UpdateRoadmapNodeCommand, UpdateRoadmapNodeResult>
    {
        public override void Configure()
        {
            Patch("/api/roadmaps/{roadmapId}/nodes/{nodeId}");
            Summary(s => s.Summary = "Update roadmap node");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            UpdateRoadmapNodeCommand req,
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
