using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmap.Delete;

namespace SSS.Web.Endpoints.Content.Roadmap.Delete
{
    public class DeleteRoadmapEndpoint(ISender sender)
        : Endpoint<DeleteRoadmapCommand, DeleteRoadmapResult>
    {
        public override void Configure()
        {
            Delete("/api/roadmaps/{roadmapId}");
            Summary(s => s.Summary = "Delete roadmap and all related data");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteRoadmapCommand req,
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
