using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.GetUserPayments;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Payments.GetUserPayments;

public sealed class GetUserPaymentsRequest
{
    [QueryParam] public int PageIndex { get; set; } = 1;
    [QueryParam] public int PageSize { get; set; } = 10;
}

public sealed class GetUserPaymentsEndpoint(ISender sender, IHttpContextAccessor httpContext)
    : Endpoint<GetUserPaymentsRequest, GetUserPaymentsResult>
{
    public override void Configure()
    {
        Get("/api/payments/user-payments");
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Get all payments for the current user");
    }

    public override async Task HandleAsync(GetUserPaymentsRequest req, CancellationToken ct)
    {
        var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        var result = await sender.Send(new GetUserPaymentsQuery(userId, req.PageIndex, req.PageSize), ct);

        await SendOkAsync(result, ct);
    }
}
