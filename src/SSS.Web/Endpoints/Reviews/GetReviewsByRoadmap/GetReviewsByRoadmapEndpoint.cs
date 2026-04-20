using FastEndpoints;
using MediatR;
using SSS.Application.Features.Reviews.GetReviewsByRoadmap;

namespace SSS.Web.Endpoints.Reviews.GetReviewsByRoadmap
{
    public class GetReviewsByRoadmapEndpoint(ISender sender)
        : Endpoint<GetReviewsByRoadmapQuery, GetReviewsByRoadmapResult>
    {
        public override void Configure()
        {
            Get("/api/reviews/roadmap/{RoadmapId}");
            Description(d => d.WithTags("Reviews"));
            Summary(s => s.Summary = "Get all reviews for a roadmap with pagination");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetReviewsByRoadmapQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
