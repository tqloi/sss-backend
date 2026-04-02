using MediatR;

namespace SSS.Application.Features.Payments.GetAllTransactions
{
    public sealed record GetAllTransactionsQuery(
        int PageIndex = 1,
        int PageSize = 20
    ) : IRequest<GetAllTransactionsResponse>;
}
