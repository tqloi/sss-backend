using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Payments.PaymentSuccess;

public sealed class PaymentSuccessCommand : IRequest<PaymentSuccessResult>
{
    [JsonIgnore]
    public string UserId { get; set; } = null!;
    public long PaymentId { get; set; }
}
