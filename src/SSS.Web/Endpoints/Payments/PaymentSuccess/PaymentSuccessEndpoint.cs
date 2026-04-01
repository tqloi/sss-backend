using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.PaymentSuccess;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.PaymentSuccess;

public sealed class PaymentSuccessRequest
{
    public long PaymentId { get; set; }
}

public sealed class PaymentSuccessEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : Endpoint<PaymentSuccessRequest, PaymentSuccessResult>
{
    public override void Configure()
    {
        Post("/api/payments/payment-success");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Mark payment as successful and activate subscription");
    }

    public override async Task HandleAsync(PaymentSuccessRequest req, CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new PaymentSuccessCommand
        {
            UserId = userId,
            PaymentId = req.PaymentId,
        }, ct);

        await SendOkAsync(result, ct);
    }
}
