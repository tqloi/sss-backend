using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Payments.GetUserPayments.Common;

namespace SSS.Application.Features.Payments.GetUserPayments;

public sealed class GetUserPaymentsHandler(
    IAppDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetUserPaymentsQuery, GetUserPaymentsResult>
{
    public async Task<GetUserPaymentsResult> Handle(GetUserPaymentsQuery request, CancellationToken ct)
    {
        var payments = await dbContext.UserPayments
            .AsNoTracking()
            .Where(p => p.UserId == request.UserId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(ct);

        var paymentDtos = mapper.Map<List<UserPaymentDto>>(payments);

        return new GetUserPaymentsResult
        {
            Payments = paymentDtos,
            TotalCount = paymentDtos.Count,
        };
    }
}
