using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.NodeContents.Update;

namespace SSS.Web.Endpoints.Content.NodeContents.Update
{
    public class UpdateNodeContentEndpoint(ISender sender)
        : Endpoint<UpdateNodeContentCommand, UpdateNodeContentResult>
    {
        public override void Configure()
        {
            Patch("/api/roadmaps/{roadmapId}/nodes/{nodeId}/contents/{contentId}");
            Summary(s => s.Summary = "Update node content");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            UpdateNodeContentCommand req,
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
