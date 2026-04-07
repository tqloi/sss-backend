using SSS.Application.Common.Dtos;

namespace SSS.Application.Features.Payments.GetAllTransactions
{
    public sealed class GetAllTransactionsResponse
    {
        public PaginatedResponse<AdminTransactionDto> Transactions { get; set; } =
            new(1, 20, 0, new List<AdminTransactionDto>());
    }

    public sealed class AdminTransactionDto
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string SubscriptionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
