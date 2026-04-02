using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.PaymentSuccess;

public sealed class PaymentSuccessHandler(IAppDbContext context, IMapper mapper)
    : IRequestHandler<PaymentSuccessCommand, PaymentSuccessResult>
{
    public async Task<PaymentSuccessResult> Handle(PaymentSuccessCommand req, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == req.PaymentId && p.UserId.Equals(req.UserId), ct)
            ?? throw new NotFoundException("Payment not found");

        if (payment.Status == PaymentStatus.Success)
        {
            return new PaymentSuccessResult
            {
                Success = true,
                Message = "Payment already processed",
                Data = mapper.Map<PaymentStatusDto>(payment),
            };
        }

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
        
        user.SubscriptionEndDate = baseDate.AddMonths(payment.SubscriptionDuration);

        await context.SaveChangesAsync(ct);

        return new PaymentSuccessResult
        {
            Success = true,
            Message = "Payment marked as successful",
            Data = mapper.Map<PaymentStatusDto>(payment),
        };
    }
}

