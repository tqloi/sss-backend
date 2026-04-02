using Net.payOS.Types;
using SSS.Application.Abstractions.External.Payment.PayOS;

namespace SSS.Infrastructure.External.Payment.PayOS;

public sealed class PayOsGateway(Net.payOS.PayOS payOs) : IPayOsGateway
{
    public async Task<PayOsCreatePaymentResponse> CreatePaymentLinkAsync(
        PayOsCreatePaymentRequest request,
        CancellationToken ct = default
    )
    {
        var items = new List<ItemData>
        {
            new("StudySense Subscription", 1, request.Amount)
        };

        var paymentData = new PaymentData(
            request.OrderCode,
            request.Amount,
            request.Description,
            items,
            request.CancelUrl,
            request.ReturnUrl
        );

        var result = await payOs.createPaymentLink(paymentData);

        return new PayOsCreatePaymentResponse
        {
            CheckoutUrl = result.checkoutUrl,
            QrCode = result.qrCode,
            PaymentLinkId = result.paymentLinkId,
        };
    }

    public Net.payOS.Types.WebhookData VerifyWebhookData(Net.payOS.Types.WebhookType webhookBody)
    {
        // Net.payOS sdk library provides dynamic signature verification 
        return payOs.verifyPaymentWebhookData(webhookBody);
    }
}
