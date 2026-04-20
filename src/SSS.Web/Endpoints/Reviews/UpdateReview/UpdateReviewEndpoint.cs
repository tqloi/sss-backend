using FastEndpoints;
using MediatR;
using SSS.Application.Features.Reviews.UpdateReview;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Reviews.UpdateReview
{
    public class UpdateReviewEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<UpdateReviewRequest, UpdateReviewResult>
    {
        public override void Configure()
        {
            Put("/api/reviews/{Id}");
            Description(d => d.WithTags("Reviews"));
            Summary(s => s.Summary = "Update an existing review (owner only)");
        }

        public override async Task HandleAsync(UpdateReviewRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new UpdateReviewCommand
            {
                Id = req.Id,
                RequesterId = userId!,
                Comment = req.Comment,
                Rating = req.Rating
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
