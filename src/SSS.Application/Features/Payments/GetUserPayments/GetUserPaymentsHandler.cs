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
        var pageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var baseQuery = dbContext.UserPayments
            .AsNoTracking()
            .Where(p => p.UserId == request.UserId && p.Status != Domain.Enums.PaymentStatus.Pending);

        var totalCount = await baseQuery.CountAsync(ct);

        var payments = await baseQuery
            .OrderByDescending(p => p.PaymentDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var paymentDtos = mapper.Map<List<UserPaymentDto>>(payments);
        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new GetUserPaymentsResult
        {
            Payments = paymentDtos,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }
}
