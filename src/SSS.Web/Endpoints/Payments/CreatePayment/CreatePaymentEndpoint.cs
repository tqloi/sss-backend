using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.CreatePayment;
using SSS.Domain.Constants;
using SSS.Domain.Enums;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace SSS.Web.Endpoints.Payments.CreatePayment;

public sealed class CreatePaymentRequest
{
    [JsonIgnore]
    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Premium;
    public decimal? Amount { get; set; } = MembershipConstants.PreMiumPlan;
    public string? Description { get; set; }
    public string? ReturnUrl { get; set; } 
    public string? CancelUrl { get; set; }
}

public sealed class CreatePaymentEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : Endpoint<CreatePaymentRequest, CreatePaymentResult>
{
    public override void Configure()
    {
        Post("/api/payments/create-payment");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Create a PayOS payment link");
    }

    public override async Task HandleAsync(CreatePaymentRequest req, CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new CreatePaymentCommand
        {
            UserId = userId,
            SubscriptionType = req.SubscriptionType,
            Amount = req.Amount,
            Description = req.Description,
            ReturnUrl = req.ReturnUrl,
            CancelUrl = req.CancelUrl,
        }, ct);

        await SendOkAsync(result, ct);
    }
}
