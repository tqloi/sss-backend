using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmap.Create;

namespace SSS.Web.Endpoints.Content.Roadmap.Create
{
    public class CreateRoadmapEndpoint(ISender sender)
        : Endpoint<CreateRoadmapCommand, CreateRoadmapResult>
    {
        public override void Configure()
        {
            Post("/api/roadmaps");
            Summary(s => s.Summary = "Create a new roadmap");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            CreateRoadmapCommand req,
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
