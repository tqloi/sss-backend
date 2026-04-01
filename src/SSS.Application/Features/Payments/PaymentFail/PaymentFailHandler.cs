using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.PaymentFail;

public sealed class PaymentFailHandler(IAppDbContext context)
    : IRequestHandler<PaymentFailCommand, PaymentFailResult>
{
    public async Task<PaymentFailResult> Handle(PaymentFailCommand req, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == req.PaymentId && p.UserId.Equals(userId), ct)
            ?? throw new NotFoundException("Payment not found");

        payment.Status = PaymentStatus.Failed;
        payment.PaymentDate = DateTime.UtcNow;

        await context.SaveChangesAsync(ct);

        return new PaymentFailResult
        {
            Success = true,
            Message = "Payment marked as failed",
            Data = new PaymentStatusDto
            {
                PaymentId = payment.Id,
                Status = payment.Status,
            },
        };
    }
}
