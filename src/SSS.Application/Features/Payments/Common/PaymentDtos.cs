using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.Common;

public sealed class CreatePaymentDto
{
    public long PaymentId { get; set; }
    public decimal Amount { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public int SubscriptionDuration { get; set; } // Duration in months
    public string CheckoutUrl { get; set; } = null!;
    public string? PaymentLinkId { get; set; }
    public string? QrCode { get; set; }
}

public sealed class PaymentStatusDto
{
    public long PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
}
