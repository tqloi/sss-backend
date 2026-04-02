namespace SSS.Application.Abstractions.External.Payment.PayOS;

public sealed class PayOsCreatePaymentRequest
{
    public long OrderCode { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; } = null!;
    public string CancelUrl { get; set; } = null!;
    public string ReturnUrl { get; set; } = null!;
}

public sealed class PayOsCreatePaymentResponse
{
    public string CheckoutUrl { get; set; } = null!;
    public string? QrCode { get; set; }
    public string? PaymentLinkId { get; set; }
}

public interface IPayOsGateway
{
    Task<PayOsCreatePaymentResponse> CreatePaymentLinkAsync(
        PayOsCreatePaymentRequest request,
        CancellationToken ct = default
    );

    Net.payOS.Types.WebhookData VerifyWebhookData(Net.payOS.Types.WebhookType webhookBody);
}
