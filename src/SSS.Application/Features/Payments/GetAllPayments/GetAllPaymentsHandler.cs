using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Payments.GetUserPayments.Common;

namespace SSS.Application.Features.Payments.GetAllPayments;

public sealed class GetAllPaymentsHandler(
    IAppDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetAllPaymentsQuery, GetAllPaymentsResult>
{
    public async Task<GetAllPaymentsResult> Handle(GetAllPaymentsQuery request, CancellationToken ct)
    {
        var payments = await dbContext.UserPayments
            .AsNoTracking()
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(ct);

        var paymentDtos = mapper.Map<List<UserPaymentDto>>(payments);

        return new GetAllPaymentsResult
        {
            Payments = paymentDtos,
            TotalCount = paymentDtos.Count,
        };
    }
}