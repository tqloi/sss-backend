using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.NodeContents.Create;

namespace SSS.Web.Endpoints.Content.NodeContents.Create
{
    public class CreateNodeContentEndpoint(ISender sender)
        : Endpoint<CreateNodeContentCommand, CreateNodeContentResult>
    {
        public override void Configure()
        {
            Post("/api/roadmaps/{roadmapId}/nodes/{nodeId}/contents");
            Summary(s => s.Summary = "Create node content");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            CreateNodeContentCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendAsync(result, statusCode: 400, ct);
                return;
            }

            await SendAsync(result, statusCode: 201, ct);
        }
    }
}
