using MediatR;
using SSS.Domain.Enums;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Payments.CreatePayment;

public sealed class CreatePaymentCommand : IRequest<CreatePaymentResult>
{
    [JsonIgnore]
    public string UserId { get; set; } = null!;

    public SubscriptionType SubscriptionType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? ReturnUrl { get; set; }
    public string? CancelUrl { get; set; }
}
