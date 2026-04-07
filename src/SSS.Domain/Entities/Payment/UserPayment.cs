using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Payment
{
    public class UserPayment
    {
        public long Id { get; set; }

        public string UserId { get; set; } = null!;

        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Premium;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "VND";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public int SubscriptionDuration { get; set; } = 1; // Duration in months (1, 6, 12)

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
