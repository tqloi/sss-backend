using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Payments.GetPaymentStatus;

public class GetPaymentStatusQuery : IRequest<GetPaymentStatusResult>
{
    public long PaymentId { get; set; }
}

public class GetPaymentStatusResult
{
    public PaymentStatus Status { get; set; }
}
