using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.Payment.PayOS;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.CancelPayment;

public sealed class CancelPaymentHandler(
    IAppDbContext context,
    IPayOsGateway payOsGateway,
    ILogger<CancelPaymentHandler> logger
) : IRequestHandler<CancelPaymentCommand, bool>
{
    public async Task<bool> Handle(CancelPaymentCommand request, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == request.PaymentId && p.UserId == request.UserId, ct);

        if (payment == null)
        {
            throw new NotFoundException("Payment record not found or you do not have permission.");
        }

        // Only allow canceling if it's currently pending
        if (payment.Status != PaymentStatus.Pending)
        {
            logger.LogInformation("Cancelation ignored because payment {Id} status is {Status}", payment.Id, payment.Status);
            return false;
        }

        payment.Status = PaymentStatus.Canceled;
        
        try
        {
            await payOsGateway.CancelPaymentLinkAsync(payment.Id, "User aborted payment", ct);
        }
        catch (Exception ex)
        {
            // Even if PayOS throws an exception (e.g., link already cancelled/expired naturally),
            // we proceed to cancel it locally.
            logger.LogWarning(ex, "Failed to cancel PayOS payment link for orderCode {Id}. Ignoring upstream error.", payment.Id);
        }

        await context.SaveChangesAsync(ct);
        return true;
    }
}
