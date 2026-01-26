using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapNodes.Delete;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.Delete
{
    public class DeleteRoadmapNodeEndpoint(ISender sender)
        : Endpoint<DeleteRoadmapNodeCommand, DeleteRoadmapNodeResult>
    {
        public override void Configure()
        {
            Delete("/api/roadmaps/{roadmapId}/nodes/{nodeId}");
            Summary(s => s.Summary = "Delete roadmap node and related contents/edges");
            Description(d => d.WithTags("RoadmapNodes"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteRoadmapNodeCommand req,
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
