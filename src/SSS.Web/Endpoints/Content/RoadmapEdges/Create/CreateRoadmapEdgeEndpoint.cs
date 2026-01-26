using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapEdges.Create;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.Create
{
    public class CreateRoadmapEdgeEndpoint(ISender sender)
        : Endpoint<CreateRoadmapEdgeCommand, CreateRoadmapEdgeResult>
    {
        public override void Configure()
        {
            Post("/api/roadmaps/{roadmapId}/edges");
            Summary(s => s.Summary = "Create a new roadmap edge with validation");
            Description(d => d.WithTags("RoadmapEdges"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            CreateRoadmapEdgeCommand req,
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
