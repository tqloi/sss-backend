using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.NodeContents.GetAll;

namespace SSS.Web.Endpoints.Content.NodeContents.GetAll
{
    public class GetNodeContentsEndpoint(ISender sender)
        : Endpoint<GetNodeContentsQuery, GetNodeContentsResult>
    {
        public override void Configure()
        {
            Get("/api/roadmaps/{roadmapId}/nodes/{nodeId}/contents");
            Summary(s => s.Summary = "Get all contents for a node ordered by OrderNo");
            Description(d => d.WithTags("NodeContents"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(
            GetNodeContentsQuery req,
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
