using FastEndpoints;
using MediatR;
using SSS.Application.Features.Payments.Webhook;

namespace SSS.Web.Endpoints.Payments.Webhook;

public sealed class PayOsWebhookEndpoint(ISender sender) 
    : Endpoint<Net.payOS.Types.WebhookType, PayOsWebhookResult>
{
    public override void Configure()
    {
        Post("/api/payments/webhook/payos");
        AllowAnonymous(); // Webhooks come from PayOS server directly
        Description(d => d.WithTags("Payments"));
        Summary(s => s.Summary = "Webhook handler for PayOS payments");
    }

    public override async Task HandleAsync(Net.payOS.Types.WebhookType req, CancellationToken ct)
    {
        var result = await sender.Send(new PayOsWebhookCommand { WebhookData = req }, ct);

        if (result.Success)
        {
            await SendOkAsync(new () { Success = true }, ct);
        }
        else
        {
            // ALWAYS return 200 OK so PayOS does not endlessly retry failed validations.
            await SendOkAsync (new () { Success = false }, ct);
        }
    }
}
