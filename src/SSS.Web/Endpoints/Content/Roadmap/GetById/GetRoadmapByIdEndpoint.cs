using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmap.GetById;

namespace SSS.Web.Endpoints.Content.Roadmap.GetById
{
    public class GetRoadmapByIdEndpoint(ISender sender)
        : Endpoint<GetRoadmapByIdQuery, GetRoadmapByIdResult>
    {
        public override void Configure()
        {
            Get("/api/roadmaps/{roadmapId}");
            Summary(s => s.Summary = "Get roadmap graph including nodes and edges");
            Description(d => d.WithTags("Roadmaps"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(
            GetRoadmapByIdQuery req,
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
