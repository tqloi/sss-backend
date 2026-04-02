using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Payments.Common;
using SSS.Application.Features.Payments.PaymentSuccess;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.PaymentFail;

public sealed class PaymentFailHandler(IAppDbContext context, IMapper mapper)
    : IRequestHandler<PaymentFailCommand, PaymentFailResult>
{
    public async Task<PaymentFailResult> Handle(PaymentFailCommand req, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == req.PaymentId, ct)
            ?? throw new NotFoundException("Payment not found");

        if (payment.Status != PaymentStatus.Pending)
        {
            return new PaymentFailResult
            {
                Success = true,
                Message = "Payment already processed",
                Data = mapper.Map<PaymentStatusDto>(payment),
            };
        }

        payment.Status = PaymentStatus.Failed;
        payment.PaymentDate = DateTime.UtcNow;

        await context.SaveChangesAsync(ct);

        return new PaymentFailResult
        {
            Success = true,
            Message = "Payment marked as failed",
            Data = mapper.Map<PaymentStatusDto>(payment),
        };
    }
}
