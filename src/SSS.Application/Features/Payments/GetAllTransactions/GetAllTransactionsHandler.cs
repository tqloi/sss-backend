using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;

namespace SSS.Application.Features.Payments.GetAllTransactions
{
    public sealed class GetAllTransactionsHandler(
        IAppDbContext dbContext
    ) : IRequestHandler<GetAllTransactionsQuery, GetAllTransactionsResponse>
    {
        public async Task<GetAllTransactionsResponse> Handle(GetAllTransactionsQuery request, CancellationToken ct)
        {
            var paymentsQuery = dbContext.UserPayments
                .AsNoTracking()
                .OrderByDescending(x => x.PaymentDate)
                .ThenByDescending(x => x.Id)
                .AsQueryable();

            var paginatedPayments = await PaginatedResponse<Domain.Entities.Payment.UserPayment>
                .CreateAsync(
                    paymentsQuery,
                    request.PageIndex,
                    request.PageSize,
                    ct
                );

            if (paginatedPayments.Items.Count == 0)
            {
                return new GetAllTransactionsResponse
                {
                    Transactions = new PaginatedResponse<AdminTransactionDto>(
                        paginatedPayments.PageIndex,
                        paginatedPayments.PageSize,
                        paginatedPayments.TotalCount,
                        new List<AdminTransactionDto>()
                    )
                };
            }

            var userIdStrings = paginatedPayments.Items
                .Select(x => x.UserId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var users = await dbContext.Users
                .AsNoTracking()
                .Where(u => userIdStrings.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName
                })
                .ToListAsync(ct);

            var usersById = users.ToDictionary(x => x.Id, x => x);

            var items = paginatedPayments.Items
                .Select(payment =>
                {
                    usersById.TryGetValue(payment.UserId, out var user);

                    var fullName = user is null
                        ? null
                        : string.Join(" ", new[] { user.FirstName, user.LastName }
                            .Where(name => !string.IsNullOrWhiteSpace(name))).Trim();

                    return new AdminTransactionDto
                    {
                        Id = payment.Id,
                        UserName = !string.IsNullOrWhiteSpace(fullName)
                            ? fullName
                            : user?.UserName,
                        UserEmail = user?.Email,
                        SubscriptionType = payment.SubscriptionType.ToString(),
                        Amount = payment.Amount,
                        Currency = payment.Currency,
                        Status = payment.Status.ToString(),
                        PaymentDate = payment.PaymentDate,
                        PaymentMethod = null
                    };
                })
                .ToList();

            return new GetAllTransactionsResponse
            {
                Transactions = new PaginatedResponse<AdminTransactionDto>(
                    paginatedPayments.PageIndex,
                    paginatedPayments.PageSize,
                    paginatedPayments.TotalCount,
                    items
                )
            };
        }
    }
}
