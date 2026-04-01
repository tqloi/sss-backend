using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.PaymentFail;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.PaymentFail;

public sealed class PaymentFailRequest
{
    public long PaymentId { get; set; }
}

public sealed class PaymentFailEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : Endpoint<PaymentFailRequest, PaymentFailResult>
{
    public override void Configure()
    {
        Post("/api/payments/payment-fail");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Mark payment as failed");
    }

    public override async Task HandleAsync(PaymentFailRequest req, CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new PaymentFailCommand
        {
            UserId = userId,
            PaymentId = req.PaymentId,
        }, ct);

        await SendOkAsync(result, ct);
    }
}
