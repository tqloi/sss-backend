using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.NodeContents.Delete;

namespace SSS.Web.Endpoints.Content.NodeContents.Delete
{
    public class DeleteNodeContentEndpoint(ISender sender)
        : Endpoint<DeleteNodeContentCommand, DeleteNodeContentResult>
    {
        public override void Configure()
        {
            Delete("/api/roadmaps/{roadmapId}/nodes/{nodeId}/contents/{contentId}");
            Summary(s => s.Summary = "Delete node content");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteNodeContentCommand req,
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
