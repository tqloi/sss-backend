using MediatR;
using System.Text.Json;

namespace SSS.Application.Features.PostHog.ReceiveWebhook
{
    public class ReceivePostHogWebhookCommand : IRequest
    {
        public JsonElement RawPayload { get; set; }
    }
}
