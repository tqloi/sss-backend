using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmaps.GraphSync;

namespace SSS.Web.Endpoints.Content.Roadmaps.GraphSync;

public class SyncRoadmapGraphEndpoint(ISender sender)
    : Endpoint<SyncRoadmapGraphRequest, SyncRoadmapGraphResult>
{
    public override void Configure()
    {
        Put("/api/roadmaps/{roadmapId}/graph");
        Summary(s => s.Summary = "Sync/update full roadmap graph (add/update/delete to match payload)");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SyncRoadmapGraphRequest req, CancellationToken ct)
    {
        var command = new SyncRoadmapGraphCommand
        {
            RoadmapId = req.RoadmapId,
            Roadmap = req.Roadmap,
            Nodes = req.Nodes,
            Contents = req.Contents,
            Edges = req.Edges
        };

        var result = await sender.Send(command, ct);

        if (!result.Success)
        {
            await SendAsync(result, statusCode: 400, ct);
            return;
        }

        await SendAsync(result, statusCode: 200, ct);
    }
}

public class SyncRoadmapGraphRequest
{
    public long RoadmapId { get; set; }
    public Application.Features.Content.Roadmaps.Common.RoadmapGraphUpdateMetadata? Roadmap { get; set; }
    public List<Application.Features.Content.Roadmaps.Common.RoadmapNodeGraphItem> Nodes { get; set; } = new();
    public List<Application.Features.Content.Roadmaps.Common.NodeContentGraphItem> Contents { get; set; } = new();
    public List<Application.Features.Content.Roadmaps.Common.RoadmapEdgeGraphItem> Edges { get; set; } = new();
}
