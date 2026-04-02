using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.GetUserPayments;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.GetUserPayments;

public sealed class GetUserPaymentsEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : Endpoint<EmptyRequest, GetUserPaymentsResult>
{
    public override void Configure()
    {
        Get("/api/payments/user-payments");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Get all payments for the current user");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new GetUserPaymentsQuery(userId), ct);

        await SendOkAsync(result, ct);
    }
}
