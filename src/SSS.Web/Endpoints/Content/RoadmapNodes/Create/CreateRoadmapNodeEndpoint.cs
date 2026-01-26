using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapNodes.Create;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.Create
{
    public class CreateRoadmapNodeEndpoint(ISender sender)
        : Endpoint<CreateRoadmapNodeCommand, CreateRoadmapNodeResult>
    {
        public override void Configure()
        {
            Post("/api/roadmaps/{roadmapId}/nodes");
            Summary(s => s.Summary = "Create a new roadmap node");
            Description(d => d.WithTags("RoadmapNodes"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            CreateRoadmapNodeCommand req,
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
