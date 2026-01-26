using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapEdges.BulkSync;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.BulkSync
{
    public class BulkSyncEdgesEndpoint(ISender sender)
        : Endpoint<BulkSyncEdgesCommand, BulkSyncEdgesResult>
    {
        public override void Configure()
        {
            Put("/api/roadmaps/{roadmapId}/edges/bulk");
            Summary(s => s.Summary = "Bulk sync roadmap edges (add/update/delete)");
            Description(d => d.WithTags("RoadmapEdges"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            BulkSyncEdgesCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendAsync(result, statusCode: 400, ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
