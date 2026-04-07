using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.RoadmapNodes.GetPreviousNodeContents;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.GetPreviousNodeContents
{
    public class GetPreviousNodeContentsEndpoint(ISender sender)
        : Endpoint<GetPreviousNodeContentsQuery, GetPreviousNodeContentsResult>
    {
        public override void Configure()
        {
            Get("/api/study-plans/{studyPlanId}/nodes/{roadmapNodeId}/previous-contents");
            Summary(s => s.Summary = "Get previous node contents in a study plan");
            Description(d => d.WithTags("RoadmapNodes"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetPreviousNodeContentsQuery req, CancellationToken ct)
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
