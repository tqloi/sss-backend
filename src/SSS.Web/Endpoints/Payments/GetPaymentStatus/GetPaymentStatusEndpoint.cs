using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.GetPaymentStatus;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.GetPaymentStatus;

public class GetPaymentStatusRequest
{
    public long Id { get; set; }
}

public sealed class GetPaymentStatusEndpoint(ISender sender, IHttpContextAccessor httpContext) 
    : Endpoint<GetPaymentStatusRequest, GetPaymentStatusResult>
{
    public override void Configure()
    {
        Get("/api/payments/{id}/status");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Get real-time payment status based on PayOS Webhook resolution");
    }

    public override async Task HandleAsync(GetPaymentStatusRequest req, CancellationToken ct)
    {
        var result = await sender.Send(new GetPaymentStatusQuery { PaymentId = req.Id }, ct);
        await SendOkAsync(result, ct);
    }
}
