using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Payments.CancelPayment;

public sealed class CancelPaymentCommand : IRequest<bool>
{
    [JsonIgnore]
    public string UserId { get; set; } = null!;
    public long PaymentId { get; set; }
}
