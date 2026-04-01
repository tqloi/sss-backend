using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.Common;

public sealed class CreatePaymentDto
{
    public long PaymentId { get; set; }
    public long OrderCode { get; set; }
    public int Amount { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public string CheckoutUrl { get; set; } = null!;
    public string? PaymentLinkId { get; set; }
    public string? QrCode { get; set; }
}

public sealed class PaymentStatusDto
{
    public long PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
}
