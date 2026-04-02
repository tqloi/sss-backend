using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.Webhook;

public sealed class PayOsWebhookHandler(
    IAppDbContext context,
    ILogger<PayOsWebhookHandler> logger,
    SSS.Application.Abstractions.External.Payment.PayOS.IPayOsGateway payOsGateway
) : IRequestHandler<PayOsWebhookCommand, PayOsWebhookResult>
{
    public async Task<PayOsWebhookResult> Handle(PayOsWebhookCommand request, CancellationToken ct)
    {
        var payload = request.WebhookData;

        try 
        {
            // Verify HMAC Signature securely via gateway
            var verifiedData = payOsGateway.VerifyWebhookData(payload);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to verify webhook payload signature");
            return new PayOsWebhookResult { Success = false }; 
        }
        
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == payload.data.orderCode, ct);

        if (payment == null)
        {
            logger.LogWarning("Webhook received for unknown orderCode: {OrderCode}", payload.data.orderCode);
            return new PayOsWebhookResult { Success = true }; // Always return true to PayOS
        }

        // Idempotency check: Process only if pending
        if (payment.Status == PaymentStatus.Success)
        {
            logger.LogInformation("Webhook ignored. orderCode {OrderCode} is already Success.", payload.data.orderCode);
            return new PayOsWebhookResult { Success = true };
        }

        // According to PayOS, code "00" indicates a successful payment
        if (payload.code == "00")
        {
            payment.Status = PaymentStatus.Success;
            payment.PaymentDate = DateTime.UtcNow;

            // Apply subscription
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == payment.UserId, ct);
            if (user != null)
            {
                var now = DateTime.UtcNow;
                var baseDate = user.SubscriptionEndDate.HasValue && user.SubscriptionEndDate > now
                    ? user.SubscriptionEndDate.Value
                    : now;

                user.SubscriptionType = payment.SubscriptionType;
                user.SubscriptionStartDate ??= now;
                user.SubscriptionEndDate = baseDate.AddMonths(payment.SubscriptionDuration);
            }
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
            payment.PaymentDate = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(ct);
        logger.LogInformation("Webhook successfully processed orderCode: {OrderCode}", payload.data.orderCode);

        return new PayOsWebhookResult { Success = true };
    }
}
