using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.GetUserPayments.Common;

public sealed class UserPaymentDto
{
    public long PaymentId { get; set; }
    public string UserId { get; set; } = null!;
    public SubscriptionType SubscriptionType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public PaymentStatus Status { get; set; }
    public int SubscriptionDuration { get; set; } // Duration in months (1, 6, 12)
    public DateTime PaymentDate { get; set; }
}
