namespace SSS.Domain.Constants;

public static class PaymentConstants
{
    // Subscription duration mapping (months)
    public static class SubscriptionDuration
    {
        public const int OneMonth = 1;
        public const int SixMonths = 6;
    }

    private static readonly Dictionary<(SSS.Domain.Enums.SubscriptionType Plan, int DurationMonths), decimal> SubscriptionPrices = new()
    {
        {(SSS.Domain.Enums.SubscriptionType.Premium, SubscriptionDuration.OneMonth), 19999m},
        {(SSS.Domain.Enums.SubscriptionType.Premium, SubscriptionDuration.SixMonths), 99999m},
    };

    public static decimal GetSubscriptionAmount(SSS.Domain.Enums.SubscriptionType plan, int durationMonths)
    {
        if (SubscriptionPrices.TryGetValue((plan, durationMonths), out var amount))
        {
            return amount;
        }

        throw new InvalidOperationException($"Unsupported subscription price for {plan} and {durationMonths} month(s).");
    }
}
