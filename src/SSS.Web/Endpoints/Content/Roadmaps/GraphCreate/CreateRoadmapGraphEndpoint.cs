using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmaps.GraphCreate;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Content.Roadmaps.GraphCreate;

public class CreateRoadmapGraphEndpoint(ISender sender, HttpContextAccessor httpContext)
    : Endpoint<CreateRoadmapGraphCommand, CreateRoadmapGraphResult>
{
    public override void Configure()
    {
        Post("/api/roadmaps/graph");
        Summary(s => s.Summary = "Create a full roadmap graph (roadmap + nodes + edges + contents)");
        Description(d => d.WithTags("Roadmaps"));
        Roles("ContentManager");
    }

    public override async Task HandleAsync(CreateRoadmapGraphCommand req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        req.Roadmap.CreateById = userId;
        var result = await sender.Send(req, ct);

        if (!result.Success)
        {
            await SendAsync(result, statusCode: 400, ct);
            return;
        }

        await SendAsync(result, statusCode: 201, ct);
    }
}
