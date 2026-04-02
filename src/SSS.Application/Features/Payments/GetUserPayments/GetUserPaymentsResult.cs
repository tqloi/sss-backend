using SSS.Application.Features.Payments.GetUserPayments.Common;

namespace SSS.Application.Features.Payments.GetUserPayments;

public sealed class GetUserPaymentsResult
{
    public List<UserPaymentDto> Payments { get; set; } = new();
    public int TotalCount { get; set; }
}
