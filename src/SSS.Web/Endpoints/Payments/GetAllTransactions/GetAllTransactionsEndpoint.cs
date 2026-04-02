using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.GetAllTransactions;

namespace SSS.Web.Endpoints.Payments.GetAllTransactions
{
    public sealed class GetAllTransactionsEndpoint(ISender sender)
        : Endpoint<GetAllTransactionsQuery, GetAllTransactionsResponse>
    {
        public override void Configure()
        {
            Get("/api/admin/transactions");
            Roles("Admin");
            Description(d => d.WithTags("Payments"));
            Summary(s =>
            {
                s.Summary = "Get all transactions";
                s.Description = "Returns paginated user payments for admin management.";
            });
        }

        public override async Task HandleAsync(GetAllTransactionsQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendOkAsync(response, ct);
        }
    }
}
