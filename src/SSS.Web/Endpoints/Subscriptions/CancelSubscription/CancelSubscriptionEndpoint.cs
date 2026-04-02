using FastEndpoints;
using MediatR;
using SSS.Application.Features.Subscriptions.CancelSubscription;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Subscriptions.CancelSubscription;

public sealed class CancelSubscriptionEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/api/users/cancel-subscription");
        Description(d => d.WithTags("Users"));
        Summary(s => s.Summary = "Downgrades the current user's active premium subscription");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new CancelSubscriptionCommand
        {
            UserId = userId
        }, ct);

        await SendOkAsync(new { success = result }, ct);
    }
}
