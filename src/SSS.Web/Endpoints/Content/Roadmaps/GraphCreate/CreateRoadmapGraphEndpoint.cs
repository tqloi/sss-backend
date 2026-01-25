using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmaps.GraphCreate;

namespace SSS.Web.Endpoints.Content.Roadmaps.GraphCreate;

public class CreateRoadmapGraphEndpoint(ISender sender)
    : Endpoint<CreateRoadmapGraphCommand, CreateRoadmapGraphResult>
{
    public override void Configure()
    {
        Post("/api/roadmaps/graph");
        Summary(s => s.Summary = "Create a full roadmap graph (roadmap + nodes + edges + contents)");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRoadmapGraphCommand req, CancellationToken ct)
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
