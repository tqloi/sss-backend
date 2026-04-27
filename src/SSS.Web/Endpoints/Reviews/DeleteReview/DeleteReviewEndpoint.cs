using FastEndpoints;
using MediatR;
using SSS.Application.Features.Reviews.DeleteReview;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Reviews.DeleteReview
{
    public class DeleteReviewEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<DeleteReviewResult>
    {
        public override void Configure()
        {
            Delete("/api/reviews/{Id}");
            Description(d => d.WithTags("Reviews"));
            Summary(s => s.Summary = "Delete a review (owner or admin)");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<long>("Id");
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = httpContext.HttpContext!.User.IsInRole("Admin");

            var command = new DeleteReviewCommand
            {
                Id = id,
                RequesterId = userId!,
                IsAdmin = isAdmin
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
