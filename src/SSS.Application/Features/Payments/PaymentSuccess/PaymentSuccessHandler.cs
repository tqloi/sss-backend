using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.PaymentSuccess;

public sealed class PaymentSuccessHandler(IAppDbContext context)
    : IRequestHandler<PaymentSuccessCommand, PaymentSuccessResult>
{
    public async Task<PaymentSuccessResult> Handle(PaymentSuccessCommand req, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == req.PaymentId && p.UserId.Equals(req.UserId), ct)
            ?? throw new NotFoundException("Payment not found");

        payment.Status = PaymentStatus.Success;
        payment.PaymentDate = DateTime.UtcNow;

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == req.UserId, ct)
            ?? throw new NotFoundException("User not found");

        var now = DateTime.UtcNow;
        var baseDate = user.SubscriptionEndDate.HasValue && user.SubscriptionEndDate > now
            ? user.SubscriptionEndDate.Value
            : now;

        user.SubscriptionType = payment.SubscriptionType;
        user.SubscriptionStartDate ??= now;
        user.SubscriptionEndDate = baseDate.AddDays(GetSubscriptionDays(payment.SubscriptionType));

        await context.SaveChangesAsync(ct);

        return new PaymentSuccessResult
        {
            Success = true,
            Message = "Payment marked as successful",
            Data = new PaymentStatusDto
            {
                PaymentId = payment.Id,
                Status = payment.Status,
            },
        };
    }

    private static int GetSubscriptionDays(SubscriptionType type)
    {
        return type switch
        {
            SubscriptionType.Premium => 30,
            SubscriptionType.Pro => 365,
            _ => 0,
        };
    }
}
