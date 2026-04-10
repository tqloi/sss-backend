using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmap.Update;

namespace SSS.Web.Endpoints.Content.Roadmap.Update
{
    public class UpdateRoadmapEndpoint(ISender sender)
        : Endpoint<UpdateRoadmapCommand, UpdateRoadmapResult>
    {
        public override void Configure()
        {
            Patch("/api/roadmaps/{id}");
            Summary(s => s.Summary = "Update roadmap metadata (partial update)");
            Description(d => d.WithTags("Roadmaps"));
            Roles("ContentManager");
        }

        public override async Task HandleAsync(
            UpdateRoadmapCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                if (string.Equals(result.Message, "Roadmap not found.", StringComparison.OrdinalIgnoreCase))
                {
                    await SendNotFoundAsync(ct);
                    return;
                }

                AddError(result.Message ?? "Roadmap update failed.");
                await SendErrorsAsync(statusCode: 400, cancellation: ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
