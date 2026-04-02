using MediatR;

namespace SSS.Application.Features.Payments.GetAllPayments;

public sealed record GetAllPaymentsQuery() : IRequest<GetAllPaymentsResult>;