using MediatR;
using Net.payOS.Types;

namespace SSS.Application.Features.Payments.Webhook;

public class PayOsWebhookCommand : IRequest<PayOsWebhookResult>
{
    public WebhookType WebhookData { get; set; } = null!;
}

public class PayOsWebhookResult
{
    public bool Success { get; set; }
}
