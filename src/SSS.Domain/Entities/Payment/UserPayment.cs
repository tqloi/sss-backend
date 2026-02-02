using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Payment
{
    public class UserPayment
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Premium;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "VND";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
