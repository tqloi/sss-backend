using MediatR;

namespace SSS.Application.Features.Payments.GetUserPayments;

public sealed record GetUserPaymentsQuery(
    string UserId,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<GetUserPaymentsResult>;
