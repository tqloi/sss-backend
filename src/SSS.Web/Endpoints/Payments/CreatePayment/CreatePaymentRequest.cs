using SSS.Domain.Constants;
using SSS.Domain.Enums;
using System.Text.Json.Serialization;

namespace SSS.Web.Endpoints.Payments.CreatePayment
{
    public sealed class CreatePaymentRequest
    {
        [JsonIgnore]
        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Premium;
        public int SubscriptionDuration { get; set; } = PaymentConstants.SubscriptionDuration.OneMonth; // 1 or 6 months
        //public string? Description { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
    }

}
