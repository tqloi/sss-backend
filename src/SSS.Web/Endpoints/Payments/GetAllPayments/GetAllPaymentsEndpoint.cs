using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.GetAllPayments;

namespace SSS.Web.Endpoints.Payments.GetAllPayments;

public sealed class GetAllPaymentsEndpoint(ISender sender)
    : EndpointWithoutRequest<GetAllPaymentsResult>
{
    public override void Configure()
    {
        Get("/api/admin/payments");
        Roles("Admin");
        //AllowAnonymous();
        Description(d => d.WithTags("Payments"));
        Summary(s =>
        {
            s.Summary = "Get all payments";
            s.Description = "Returns all payments for admin management.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new GetAllPaymentsQuery(), ct);
        await SendOkAsync(result, ct);
    }
}