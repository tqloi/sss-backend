using SSS.Application.Features.Payments.GetUserPayments.Common;

namespace SSS.Application.Features.Payments.GetAllPayments;

public sealed class GetAllPaymentsResult
{
    public List<UserPaymentDto> Payments { get; set; } = new();
    public int TotalCount { get; set; }
}