using MediatR;

namespace SSS.Application.Features.Payments.GetUserPayments;

public sealed record GetUserPaymentsQuery(
    string UserId
) : IRequest<GetUserPaymentsResult>;
