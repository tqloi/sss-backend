using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.Payment.PayOS;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.GetPaymentStatus;

public sealed class GetPaymentStatusQueryHandler(
    IAppDbContext context,
    IPayOsGateway payOsGateway,
    ILogger<GetPaymentStatusQueryHandler> logger
) : IRequestHandler<GetPaymentStatusQuery, GetPaymentStatusResult>
{
    public async Task<GetPaymentStatusResult> Handle(GetPaymentStatusQuery request, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == request.PaymentId, ct)
            ?? throw new NotFoundException("Payment not found");

        // If it's already processed via Webhook or previous sync, just return it
        if (payment.Status != PaymentStatus.Pending)
        {
            return new GetPaymentStatusResult { Status = payment.Status };
        }

        // It is Pending. We should explicitly sync with PayOS because Webhook might not have arrived
        // or the environment is local and cannot receive webhooks!
        try
        {
            var payOsData = await payOsGateway.GetPaymentLinkInformationAsync(payment.Id, ct);
            
            bool statusChanged = false;

            if (payOsData.status == "PAID")
            {
                payment.Status = PaymentStatus.Success;
                statusChanged = true;
                logger.LogInformation("Payment {Id} explicitly synced as Success via PayOS status check", payment.Id);
            }
            else if (payOsData.status == "CANCELLED")
            {
                payment.Status = PaymentStatus.Canceled;
                statusChanged = true;
                logger.LogInformation("Payment {Id} explicitly synced as Canceled via PayOS status check", payment.Id);
            }
            
            if (statusChanged)
            {
                await context.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to verify payment status from PayOS for {Id}. Ignoring.", payment.Id);
            // We ignore errors here. The status remains Pending.
        }

        return new GetPaymentStatusResult
        {
            Status = payment.Status
        };
    }
}
