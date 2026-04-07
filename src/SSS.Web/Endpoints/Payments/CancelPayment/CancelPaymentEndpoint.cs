using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.CancelPayment;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.CancelPayment;

public sealed class CancelPaymentEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/api/payments/{id}/cancel");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Explicitly cancel a pending payment on system and PayOS");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var paymentId = Route<long>("id");

        var result = await sender.Send(new CancelPaymentCommand
        {
            UserId = userId,
            PaymentId = paymentId
        }, ct);

        await SendOkAsync(new { success = result }, ct);
    }
}
