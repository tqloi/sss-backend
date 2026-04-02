using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;

namespace SSS.Application.Features.Payments.GetPaymentStatus;

public sealed class GetPaymentStatusQueryHandler(IAppDbContext context) 
    : IRequestHandler<GetPaymentStatusQuery, GetPaymentStatusResult>
{
    public async Task<GetPaymentStatusResult> Handle(GetPaymentStatusQuery request, CancellationToken ct)
    {
        var payment = await context.UserPayments
            .FirstOrDefaultAsync(p => p.Id == request.PaymentId, ct)
            ?? throw new NotFoundException("Payment not found");

        return new GetPaymentStatusResult
        {
            Status = payment.Status
        };
    }
}
