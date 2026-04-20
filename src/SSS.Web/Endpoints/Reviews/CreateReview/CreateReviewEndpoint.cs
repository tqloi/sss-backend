using FastEndpoints;
using MediatR;
using SSS.Application.Features.Reviews.CreateReview;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Reviews.CreateReview
{
    public class CreateReviewEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<CreateReviewRequest, CreateReviewResult>
    {
        public override void Configure()
        {
            Post("/api/reviews");
            Description(d => d.WithTags("Reviews"));
            Summary(s => s.Summary = "Create a new review for a roadmap");
        }

        public override async Task HandleAsync(CreateReviewRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CreateReviewCommand
            {
                RoadmapId = req.RoadmapId,
                ReviewerId = userId!,
                Comment = req.Comment,
                Rating = req.Rating
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
