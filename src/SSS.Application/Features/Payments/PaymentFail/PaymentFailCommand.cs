using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Payments.PaymentFail;

public sealed class PaymentFailCommand : IRequest<PaymentFailResult>
{
    [JsonIgnore]
    public string UserId { get; set; } = null!;
    public long PaymentId { get; set; }
}
